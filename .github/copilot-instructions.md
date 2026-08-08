# Copilot Instructions

## Directrices del proyecto
- El asistente de ecommerce debe cubrir dos contextos: en la página principal, ofrecer un resumen del catálogo; en el detalle, recibir el producto actual desde la aplicación y responder con sus detalles, reseñas y productos similares por categoría, precio y descripción. El usuario no debe conocer identificadores internos.
- El asistente debe diferenciar objetivos por contexto: en Catalog orienta al usuario hacia el producto adecuado; en ProductDetail actúa como vendedor para favorecer la compra, con comunicación personalizada usando datos autorizados como el nombre del usuario autenticado.

## Capacidades de Eventos
- Para nuevas capacidades de eventos se debe preservar el flujo existente de pedidos: reutilizar la infraestructura RabbitMQ probada, pero crear un flujo de comportamiento separado con contratos, colas y consumidores independientes.
- Las capacidades de eventos deben documentarse de extremo a extremo: punto de registro, nombre y cuerpo, emisor, consumidor, procesamiento, persistencia y uso posterior.