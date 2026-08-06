# Integración RabbitMQ

## Alcance de B6

La API publica el evento de integración `OrderCreatedIntegrationEvent` después de persistir el pedido, sus líneas y el PaymentIntent. El evento se publica en el exchange topic durable `ecommerce.integration` con la routing key `orders.created` y contenido JSON persistente.

El contrato está en `Ecommerce.Application.Models.Messaging` y el transporte se mantiene en Infrastructure mediante `IIntegrationEventPublisher`. Los consumidores downstream pueden crear sus propias colas y enlazarlas al exchange sin introducir dependencias de RabbitMQ en Application.

## Configuración

Las opciones se leen de la sección `RabbitMq` y admiten configuración por variables de entorno con la convención de .NET:

- `RabbitMq__HostName`
- `RabbitMq__Port`
- `RabbitMq__UserName`
- `RabbitMq__Password`
- `RabbitMq__VirtualHost`
- `RabbitMq__ExchangeName`

Para desarrollo fuera de Docker el host por defecto es `localhost`. En Compose, la API usa el nombre de servicio `rabbitmq`. El broker local expone AMQP en `5672` y la interfaz de administración en `15672`.

## Ejecución local

```powershell
docker compose up -d postgres rabbitmq
docker compose up api
```

La interfaz de administración queda disponible en `http://localhost:15672` con las credenciales configuradas por `RABBITMQ_DEFAULT_USER` y `RABBITMQ_DEFAULT_PASS` (por defecto `guest` únicamente para desarrollo local).

## Limitaciones y siguiente trabajo

La publicación se realiza después del guardado de la transacción de negocio. Si RabbitMQ está temporalmente caído, el pedido no se revierte y se registra un warning; por tanto, B6 no garantiza entrega eventual. El patrón outbox, reintentos, timeouts, dead-letter y procesamiento idempotente corresponden a B6.1 y B6.2. La evolución explícita de contratos corresponde a B6.3.

No se ha añadido todavía un worker ni una cola de consumo: B6 entrega el backbone de publicación para que esos componentes se incorporen posteriormente sin mover la lógica de negocio.
