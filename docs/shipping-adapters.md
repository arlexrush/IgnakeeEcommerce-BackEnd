# Adaptadores de envío

El checkout depende de `IShippingManagementService`, mientras que los transportistas se integran mediante `Ecommerce.Domain.Contracts.IShippingCarrier`. El contrato del dominio solo intercambia `ShippingQuoteRequest` y `ShippingQuote`; no expone HTTP, XML ni modelos propietarios de un transportista.

## Flujo

1. `ShippingManagementService` recibe todos los `IShippingCarrier` registrados por inyección de dependencias.
2. Cada adaptador obtiene una tarifa usando su integración existente.
3. Las tarifas se consultan en paralelo y se selecciona la menor disponible.
4. El gestor devuelve `PropertyInformation`, que mantiene el contrato usado por `CreateOrderCommandHandler`.
5. El checkout persiste el operador, la tarifa y el peso como antes.

## Adaptadores registrados

- `MrwShippingCarrier`: traduce la respuesta de `IMrwService` a `ShippingQuote`.
- `SeurShippingCarrier`: preparado con el mismo contrato común, pero deshabilitado hasta completar la API de SEUR.

MRW es el único adaptador registrado por defecto. SEUR se registra únicamente cuando `SeurSettings:Enabled` es `true`. Actualmente debe permanecer en `false` porque el repositorio todavía no contiene el endpoint, autenticación ni formato de respuesta oficial de SEUR.

La implementación de Correos se ha retirado temporalmente del runtime para reconstruirla desde cero cuando se disponga de su documentación oficial. El contrato `ICorreosService` y sus modelos XML se conservan de forma temporal porque todavía son referencias compartidas por otros servicios; no hay ningún `IShippingCarrier` ni servicio de Correos registrado en DI.

Para añadir otro transportista, implementar `IShippingCarrier`, traducir su respuesta a `ShippingQuote` y registrar la implementación como `IShippingCarrier` en `InfrastructureServiceRegistration`. Para un país nuevo, el adaptador puede usar su propio cliente, autenticación y modelos internos sin exponerlos al dominio. No es necesario modificar el checkout ni usar reflexión.

Las operaciones de etiquetas y preregistro de las integraciones existentes se mantienen en sus servicios específicos; el alcance de B5 conecta el cálculo de tarifa utilizado por el checkout mediante el contrato común.
