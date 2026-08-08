# Orquestación de IA

## Alcance

B8 añade una capa de orquestación de IA dentro de `Ecommerce.Api`. Está aislada en `AiOrchestration` y no incorpora reglas de negocio: sus herramientas delegan en las consultas MediatR existentes. La primera superficie es un asistente de catálogo de solo lectura.

## Endpoint

- URL: `POST /api/v1/AiAssistant/ask`
- Autenticación: obligatoria mediante la configuración global de la API (JWT local o Entra ID cuando está habilitado).
- Cuerpo de catálogo: `{ "message": "¿Qué productos hay?", "pageContext": { "kind": "catalog" } }`
- Cuerpo de detalle: `{ "message": "¿Qué opinan los compradores?", "pageContext": { "kind": "productDetail", "productId": 42 } }`
- Respuesta: `{ "message": "..." }`
- Estado `503`: la integración está deshabilitada o no se ha configurado un endpoint de proyecto de Microsoft Foundry.

`productId` es metadato técnico enviado por la vista de detalle, que ya conoce el producto mostrado; nunca se pide ni se muestra al usuario. Si no se indica contexto, se usa `catalog`.

## Contextos de navegación

- `catalog`: el backend aporta hasta doce productos activos como resumen de la página principal.
- `productDetail`: el backend valida el producto actual y aporta sus datos públicos, reseñas y hasta cuatro alternativas activas de la misma categoría en una banda de precio de ±20 %.

Los identificadores, nombres de reseñadores y demás datos técnicos no se incluyen en el contexto del modelo. El asistente debe usar los datos enviados por la aplicación y no inventar información ausente.

## Objetivo y personalización

- En `catalog`, el asistente ayuda a descubrir el producto adecuado: identifica necesidades, recomienda opciones existentes y pide un criterio breve cuando es necesario.
- En `productDetail`, actúa como vendedor consultivo: explica beneficios verificables, usa reseñas y alternativas disponibles, y facilita una decisión de compra sin presión ni condiciones inventadas.
- El backend obtiene el nombre público exclusivamente de los claims autenticados `name`, `given_name` o `preferred_username`. El cliente no envía perfil alguno; no se usan correo ni identificadores y, si falta un nombre seguro, el tono es neutro.
- Cuando el usuario autenticado ha dado consentimiento explícito para eventos de comportamiento, el backend aporta al asistente un resumen acotado: contadores, categorías preferidas, hasta cinco productos recientes y rango de precios observados. El historial detallado permite construir este resumen, pero no se envía completo al modelo ni contiene identificadores de usuario. El asistente usa el contexto de forma discreta para adaptar recomendaciones desde catálogo y nunca debe mencionar seguimiento, perfiles ni contadores.
- El historial detallado se conserva mientras exista consentimiento. Al retirarlo se elimina el perfil y todo su historial asociado.
- La agregación se procesa de forma asíncrona desde RabbitMQ; una interacción reciente puede no estar todavía disponible en el contexto del asistente.

## Herramientas disponibles

| Herramienta | Función |
| --- | --- |
| `GetProductCatalogAsync` | Consulta el catálogo de productos. |
| `GetProductAsync` | Consulta un producto por identificador positivo. |
| `GetCategoriesAsync` | Consulta las categorías de catálogo. |
| `GetCountriesAsync` | Consulta los países disponibles para checkout y envío. |

No se exponen pagos, pedidos, carritos, identidad, administración ni operaciones mutables. Las respuestas deben provenir de las herramientas cuando la pregunta requiera datos del catálogo.

## Configuración

La configuración de base se mantiene deshabilitada:

- `AiOrchestration__Enabled=true`
- `AiOrchestration__FoundryProjectEndpoint=https://<recurso>.services.ai.azure.com/api/projects/<proyecto>`
- `AiOrchestration__ModelDeploymentName=gpt-5-mini`

`gpt-5-mini` es el valor de referencia para llamadas de herramientas y razonamiento. `gpt-4.1-mini` es una alternativa adecuada para cargas con prioridad de baja latencia. El valor es siempre el nombre del despliegue configurado en el proyecto Foundry.

La autenticación se realiza con `DefaultAzureCredential`. Para producción debe asignarse una identidad administrada con el mínimo privilegio sobre el proyecto Foundry; no se deben guardar claves, tokens ni secretos en `appsettings*.json` ni en el repositorio. En desarrollo, la identidad de Azure CLI puede proporcionar las credenciales.

La dependencia `Microsoft.Agents.AI.AzureAI` se instala actualmente con `--prerelease`/canal de versión previa. Debe revisarse su versión antes de cada actualización de producción.

## Operación y seguridad

- Mantener `AiOrchestration:Enabled` en `false` hasta que el proyecto, el despliegue de modelo y la identidad estén preparados.
- Restringir CORS a los orígenes autorizados antes de producción.
- Registrar solo metadatos operativos; no registrar prompts ni respuestas que puedan contener datos personales.
- Evaluar y probar cualquier nueva herramienta antes de exponerla. Las mutaciones requieren autorización específica, confirmación humana e idempotencia cuando proceda.

## Verificación

`AiOrchestrationTests` valida que se conservan exactamente cuatro herramientas con descripción, que el identificador de producto es positivo, que un detalle requiere producto, que el perfil usa solo el nombre autenticado permitido y que el adaptador no intenta llamar a Foundry sin configuración.
