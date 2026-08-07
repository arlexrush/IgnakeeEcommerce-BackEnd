# MCP API

## Alcance

B7 expone una superficie MCP delgada sobre la API ecommerce existente. La implementación usa el SDK oficial `ModelContextProtocol.AspNetCore` y delega las operaciones en consultas MediatR de `Ecommerce.Application`.

## Endpoint

- URL: `/api/mcp`
- Transporte: Streamable HTTP
- Autenticación: obligatoria mediante el esquema de autenticación configurado en la API (JWT local o Entra ID cuando está habilitado).
- Autorización: el endpoint requiere un usuario autenticado. No se exponen operaciones administrativas ni mutaciones desde B7.

El agente MCP debe conectarse usando la URL HTTPS del entorno. En desarrollo puede usarse la URL local configurada por el perfil de lanzamiento, siempre que el cliente MCP pueda acceder a ella.

## Herramientas

| Herramienta | Parámetros | Descripción |
| --- | --- | --- |
| `GetProductCatalogAsync` | Ninguno | Devuelve los productos disponibles en el catálogo. |
| `GetProductAsync` | `productId` entero positivo | Devuelve un producto por identificador. |
| `GetCategoriesAsync` | Ninguno | Devuelve las categorías del catálogo. |
| `GetCountriesAsync` | Ninguno | Devuelve los países configurados para checkout y envío. |

Las descripciones de las herramientas y sus parámetros forman parte del contrato que recibe el agente MCP.

## Diseño y seguridad

- Las herramientas son de solo lectura para limitar el riesgo de acciones no deseadas por un agente.
- La lógica de negocio no se duplica: cada herramienta usa `IMediator` y los handlers existentes.
- El endpoint conserva la autenticación global de la API.
- Pagos, identidad, administración, carrito y cambios de estado quedan fuera de B7.
- CORS sigue la política de la API; para producción debe restringirse a los clientes autorizados antes de publicar el endpoint.

## Verificación

- La API compila correctamente en .NET 8.
- `EcommerceMcpToolsTests` valida el registro de las cuatro herramientas, sus descripciones y la validación del identificador de producto.

## Evolución

Las nuevas capacidades deben añadirse como herramientas pequeñas que deleguen en la capa de aplicación. Las operaciones mutables deben incorporar autorización específica, validación, idempotencia cuando corresponda y pruebas antes de exponerse a agentes.
