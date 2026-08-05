# Autenticación con Microsoft Entra ID

La API acepta el JWT local existente y, de forma opcional, tokens bearer emitidos por Microsoft Entra ID. La autenticación Entra no reemplaza el perfil ecommerce almacenado en `User`.

## Configuración

Definir estas variables mediante el entorno de ejecución o un secreto local no versionado:

```text
AzureAd__Enabled=true
AzureAd__Instance=https://login.microsoftonline.com/
AzureAd__TenantId=<tenant-id>
AzureAd__ClientId=<api-application-client-id>
```

`AzureAd__Enabled` permanece en `false` por defecto para que el desarrollo local siga usando el flujo de login existente hasta registrar la aplicación en Entra.

## Registro de la API en Entra

1. Crear un registro de aplicación para esta API en Microsoft Entra ID.
2. Usar el `TenantId` y el `Application (client) ID` en la configuración anterior.
3. En **Expose an API**, configurar el identificador de aplicación y exponer un scope como `access_as_user`.
4. Conceder ese scope a la aplicación cliente que llamará a la API.
5. Solicitar un access token para la API; no usar un ID token como bearer token de la API.

La API valida issuer, audience, firma y expiración mediante `Microsoft.Identity.Web`. No se almacenan secretos ni tokens en el repositorio.

## Asociación con el perfil ecommerce

La aplicación conserva `User` y sus datos de negocio en PostgreSQL. Para una petición autenticada por Entra, la resolución de sesión usa `preferred_username` y, como fallback, el correo del token para buscar el `User.UserName` existente. La integración no crea ni sincroniza perfiles automáticamente.

Por tanto, el perfil ecommerce debe existir con el mismo identificador antes de utilizar operaciones que dependen del usuario, como pedidos o dirección. La asociación automática de cuentas requiere una decisión de negocio y queda fuera de B4.

## Verificación local

Sin un tenant real se puede validar compilación y pruebas con:

```powershell
dotnet build EcommerceSolution.sln
dotnet test tests/Ecommerce.IntegrationTests/Ecommerce.IntegrationTests.csproj
```

Con un tenant real, obtener un token mediante Authorization Code o Client Credentials según el cliente y comprobar que una petición sin token devuelve `401` y una petición con un access token válido accede a un endpoint protegido.

Referencias oficiales:

- [Quickstart: Protect an ASP.NET Core Web API](https://learn.microsoft.com/entra/msidweb/getting-started/quickstart-webapi)
- [Microsoft identity platform: protected web API](https://learn.microsoft.com/entra/identity-platform/scenario-protected-web-api-overview)
