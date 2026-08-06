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

Para desarrollo fuera de Docker el host por defecto es `localhost`. En Compose, API y worker usan el nombre de servicio `rabbitmq`. El broker local expone AMQP en `5672` y administración en `15672`.

## Ejecución local

```powershell
docker compose up -d postgres rabbitmq
docker compose up --build api worker
```

El worker aplica las migraciones de PostgreSQL al iniciar. La interfaz de administración queda disponible en `http://localhost:15672` con `RABBITMQ_DEFAULT_USER` y `RABBITMQ_DEFAULT_PASS`.

## Evolución de contratos

Los consumidores deben comprobar `EventType` y `ContractVersion` antes de ejecutar el payload. Una nueva versión debe introducir compatibilidad explícita en el worker; no se debe reinterpretar silenciosamente el payload de una versión anterior.
