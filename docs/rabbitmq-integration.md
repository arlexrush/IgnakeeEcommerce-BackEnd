# Integración RabbitMQ

## Alcance completado: B6, B6.1, B6.2 y B6.3

La API publica `OrderCreatedIntegrationEvent` después de persistir el pedido, sus líneas y el PaymentIntent. El evento usa un envelope versionado con:

- `MessageId` para deduplicación.
- `EventType` (`orders.created`).
- `ContractVersion` (`1`).
- `OccurredOnUtc`.
- `Payload`.

El mensaje se publica como JSON persistente en el exchange topic durable `ecommerce.integration` con routing key `orders.created`.

El worker `src/Workers/Ecommerce.Messaging.Worker` consume el evento como un proceso independiente. Su procesamiento inicial registra el evento y guarda el `MessageId` en PostgreSQL. La clave primaria impide procesar de nuevo el mismo mensaje después de un redelivery o reinicio.

## Topología

| Elemento | Nombre | Propósito |
| --- | --- | --- |
| Exchange principal | `ecommerce.integration` | Publicación de eventos topic. |
| Cola principal | `ecommerce.orders.created` | Consumo de pedidos creados. |
| Cola retry | `ecommerce.orders.created.retry` | Backoff mediante TTL y retorno al exchange principal. |
| Exchange dead-letter | `ecommerce.integration.dlx` | Entrada de mensajes rechazados o agotados. |
| Cola dead-letter | `ecommerce.orders.created.dlq` | Inspección y recuperación manual. |

## Eventos de comportamiento

El flujo de comportamiento es independiente del flujo de pedidos: no consume ni modifica `orders.created`. La API autenticada ofrece los siguientes endpoints:

- `PUT /api/v1/behavior/consent`: registra o retira el consentimiento explícito del usuario autenticado.
- `POST /api/v1/behavior/events`: publica una acción únicamente si ese consentimiento está activo. El identificador del usuario se obtiene de las claims y nunca del cuerpo de la solicitud.

Las acciones admitidas son `CatalogViewed`, `ProductViewed`, `ProductAddedToCart` y `CheckoutStarted`. La aplicación puede asociar hasta veinte identificadores técnicos de producto a un evento. El worker resuelve esas referencias en catálogo y persiste una instantánea confiable de nombre, categoría y precio; no acepta esos datos desde el navegador. Se publican como `BehaviorRecordedIntegrationEvent` con tipo `behavior.recorded` y versión `1`.

### Catálogo de eventos e instrumentación

Todos los eventos se registran desde la interfaz autenticada mediante `POST /api/v1/behavior/events`, después de que la interacción ocurra realmente. La interfaz debe enviar el token del usuario y no debe enviar identificador de usuario, precio, nombre ni categoría: esos datos son responsabilidad del backend y del worker.

| Acción (`action`) | Cuándo la emite la interfaz | `productIds` | `categoryId` | Resultado esperado |
| --- | --- | --- | --- | --- |
| `CatalogViewed` | Al mostrar la página principal del catálogo al usuario. Debe emitirse una vez por carga de vista, no por cada renderizado interno. | Opcional: productos destacados que se hayan mostrado, máximo 20. | Opcional. | Registra interés de exploración y, si existen productos, afinidades iniciales. |
| `ProductViewed` | Cuando el detalle de producto se muestra efectivamente al usuario. | Obligatorio: el producto mostrado. | Opcional; el worker la resuelve desde catálogo. | Registra interés en producto, categoría y precio. |
| `ProductAddedToCart` | Solo después de que la operación de añadir al carrito haya terminado correctamente en la aplicación. | Obligatorio: el producto añadido. | No enviar. | Registra intención de compra y afinidad. |
| `CheckoutStarted` | Cuando el usuario entra de forma efectiva al checkout con un carrito válido. | Obligatorio: productos que forman el carrito en ese momento. | No enviar. | Registra intención de compra y precios/categorías del carrito. |

El nombre de la acción se serializa como cadena JSON porque la API configura `JsonStringEnumConverter`. Ejemplos de cuerpo:

```json
{ "action": "CatalogViewed", "productIds": [42, 45] }
```

```json
{ "action": "ProductViewed", "productIds": [42] }
```

```json
{ "action": "ProductAddedToCart", "productIds": [42] }
```

```json
{ "action": "CheckoutStarted", "productIds": [42, 45, 51] }
```

`categoryId` está disponible únicamente como metadato técnico opcional; no es necesario para producto, carrito ni checkout, porque el worker toma la categoría vigente del catálogo. Los identificadores técnicos se obtienen de la vista o del carrito de la propia aplicación y nunca se presentan al usuario como parte de la experiencia.

| Elemento | Nombre | Propósito |
| --- | --- | --- |
| Cola principal | `ecommerce.behavior.recorded` | Consumo aislado de eventos de comportamiento. |
| Cola retry | `ecommerce.behavior.recorded.retry` | Backoff por TTL del flujo de comportamiento. |
| Exchange dead-letter | `ecommerce.behavior.dlx` | Entrada para eventos de comportamiento incompatibles o agotados. |
| Cola dead-letter | `ecommerce.behavior.recorded.dlq` | Inspección y recuperación manual del flujo de comportamiento. |

`RabbitMqBehaviorWorker` es un `BackgroundService` registrado junto al worker de pedidos, pero dispone de handler, opciones (`BehaviorMessagingWorker`), tablas de idempotencia y perfil agregado propios. El perfil mantiene consentimiento, contadores, rango de precios observados y última actividad. Los eventos detallados permiten identificar afinidades por producto y categoría, además de intereses de carrito y checkout.

El historial detallado se conserva mientras el consentimiento esté activo. Al invocar `PUT /api/v1/behavior/consent` con `granted: false`, la API elimina el perfil y todos los eventos detallados existentes del usuario; desde ese momento no publica eventos nuevos.

### Emisión, consumo y persistencia

| Fase | Componente responsable | Qué hace |
| --- | --- | --- |
| Consentimiento | `BehaviorTrackingController.SetConsentAsync` | `PUT /api/v1/behavior/consent` crea o activa `BehaviorProfile` cuando `granted` es `true`. Con `false`, elimina el perfil y todos sus `BehaviorEvent` de forma irreversible. |
| Registro HTTP | Interfaz autenticada → `BehaviorTrackingController.TrackAsync` | La interfaz registra una interacción real en `POST /api/v1/behavior/events`. El controlador obtiene el usuario de las claims, comprueba `HasConsented`, filtra IDs inválidos/duplicados y no acepta identidad ni atributos de catálogo desde el cuerpo. |
| Publicación | `RabbitMqIntegrationEventPublisher` | Envuelve el payload en `IntegrationEventEnvelope` con `MessageId`, `EventType`, versión y fecha. Publica JSON persistente en `ecommerce.integration` con routing key `behavior.recorded`. |
| Consumo | `RabbitMqBehaviorWorker` (`BackgroundService`) | Declara la topología propia, consume `ecommerce.behavior.recorded`, valida `behavior.recorded` versión `1`, aplica timeout, reintentos, DLQ y acknowledgement manual. |
| Idempotencia | `ProcessedBehaviorMessages` | Antes de procesar, el worker comprueba `MessageId`. Tras completar la transacción registra el mensaje; un redelivery con el mismo identificador se omite. |
| Enriquecimiento | `BehaviorRecordedEventHandler` | Comprueba que el perfil conserva consentimiento, obtiene cada producto por ID desde `Products` y su categoría, y descarta IDs que no correspondan a catálogo. |
| Perfil agregado | `BehaviorProfiles` | Actualiza contadores por acción, mínimo/máximo de precio observado y última actividad. No guarda datos de pago. |
| Historial detallado | `BehaviorEvents` | Inserta una fila por producto válido con acción, producto, nombre, categoría, precio e instante. Estas instantáneas permiten calcular afinidades aunque el catálogo cambie posteriormente. |

El payload publicado en RabbitMQ tiene esta forma conceptual. `messageId` lo genera el publicador, mientras que `userId` procede de claims y no de la interfaz:

```json
{
  "messageId": "<guid>",
  "eventType": "behavior.recorded",
  "contractVersion": 1,
  "occurredOnUtc": "2026-08-08T12:00:00+00:00",
  "payload": {
	"userId": "<claim-sub-or-nameidentifier>",
	"action": "ProductViewed",
	"productIds": [42],
	"categoryId": null,
	"occurredOnUtc": "2026-08-08T12:00:00+00:00"
  }
}
```

La secuencia completa es: **interfaz autenticada → endpoint HTTP → publisher → exchange `ecommerce.integration` → cola `ecommerce.behavior.recorded` → `RabbitMqBehaviorWorker` → `BehaviorRecordedEventHandler` → PostgreSQL**. El flujo de `orders.created` no comparte cola, handler, DLQ ni tabla de idempotencia con este flujo.

### Uso posterior por el asistente

Cuando el usuario autenticado envía una pregunta a `POST /api/v1/AiAssistant/ask`, `FoundryAiAssistant` llama a `AiAssistantBehaviorProfileProvider`. El proveedor solo devuelve datos si encuentra un `BehaviorProfile` con el mismo identificador de claim y `HasConsented = true`.

| Origen PostgreSQL | Dato entregado al agente | Uso permitido |
| --- | --- | --- |
| `BehaviorProfiles` | Contadores, rango de precios observado y última actividad. | Ajustar el nivel de exploración y presentar opciones compatibles con el presupuesto inferido. |
| `BehaviorEvents` | Hasta tres categorías más frecuentes y hasta cinco productos recientes. | Priorizar productos y categorías de interés desde catálogo o aportar alternativas relevantes en detalle. |
| `BehaviorEvents` | Historial completo, IDs internos y `UserId`. | No se entrega al agente. |

El proveedor entrega un resumen acotado y de solo lectura; no modifica carrito, pedido, precio, stock, impuestos ni permisos. Las instrucciones de `FoundryAiAssistant` obligan a usarlo discretamente: el agente no debe revelar que existe seguimiento, mencionar perfiles o contadores, ni inferir atributos sensibles. La API puede tardar en reflejar una interacción recién emitida porque el consumo de RabbitMQ es asíncrono.

El worker usa `prefetch` configurable y acknowledgement manual. El `ack` solo se ejecuta después del procesamiento y de guardar la marca de idempotencia.

## Retry, timeout y dead-letter

- Los mensajes JSON inválidos o con una versión no soportada van directamente a DLQ.
- Los errores de procesamiento se reintentan hasta `MaxRetryAttempts`.
- La cola retry aplica `RetryDelayMilliseconds` mediante TTL.
- El procesamiento usa un `CancellationToken` vinculado al shutdown y `ProcessingTimeoutSeconds`.
- Los mensajes que superan el máximo de intentos se publican en DLQ.

La política no garantiza exactly-once para efectos externos al PostgreSQL; los handlers que añadan efectos externos deben ser idempotentes usando `MessageId`.

## Configuración

Las opciones RabbitMQ usan estas variables .NET:

- `RabbitMq__HostName`
- `RabbitMq__Port`
- `RabbitMq__UserName`
- `RabbitMq__Password`
- `RabbitMq__VirtualHost`
- `RabbitMq__ExchangeName`

Las opciones del worker usan:

- `MessagingWorker__QueueName`
- `MessagingWorker__RetryQueueName`
- `MessagingWorker__DeadLetterExchangeName`
- `MessagingWorker__DeadLetterQueueName`
- `MessagingWorker__PrefetchCount`
- `MessagingWorker__MaxRetryAttempts`
- `MessagingWorker__RetryDelayMilliseconds`
- `MessagingWorker__ProcessingTimeoutSeconds`

El flujo de comportamiento usa el mismo tipo de parámetros bajo el prefijo `BehaviorMessagingWorker__`.

Para desarrollo fuera de Docker el host por defecto es `localhost`. En Compose, API y worker usan el nombre de servicio `rabbitmq`. El broker local expone AMQP en `5672` y administración en `15672`.

## Ejecución local

```powershell
docker compose up -d postgres rabbitmq
docker compose up --build api worker
```

El worker aplica las migraciones de PostgreSQL al iniciar. La interfaz de administración queda disponible en `http://localhost:15672` con `RABBITMQ_DEFAULT_USER` y `RABBITMQ_DEFAULT_PASS`.

## Evolución de contratos

Los consumidores deben comprobar `EventType` y `ContractVersion` antes de ejecutar el payload. Una nueva versión debe introducir compatibilidad explícita en el worker; no se debe reinterpretar silenciosamente el payload de una versión anterior.
