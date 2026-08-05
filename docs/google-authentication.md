# Autenticación externa con Google

La API permite autenticarse con una cuenta Google/Gmail mediante ASP.NET Core Identity. La integración es opcional: si no están configuradas las dos variables de Google, los endpoints locales y Entra ID continúan funcionando sin cambios.

## Variables de entorno

Configurar en el entorno de ejecución, `.env` local no versionado, User Secrets o el gestor de secretos del despliegue:

```text
Authentication__Google__ClientId=<google-client-id>
Authentication__Google__ClientSecret=<google-client-secret>
Authentication__Google__CallbackPath=/signin-google
Authentication__PublicBaseUrl=https://api.example.com
# Optional: comma-separated IP addresses of trusted reverse proxies
ForwardedHeaders__KnownProxies=10.0.0.10
```

No añadir valores reales a `appsettings.json`, `.env.example` ni al control de código.

## Google Cloud

1. Crear un proyecto en Google Cloud.
2. Configurar la pantalla de consentimiento OAuth.
3. Crear un cliente OAuth de tipo aplicación web.
4. Añadir como URI de redirección:
   - Desarrollo: `http://localhost:5204/signin-google`
   - Producción: `https://<dominio-api>/signin-google`
5. Guardar el Client ID y el Client Secret como variables de entorno.

La URI debe coincidir exactamente con la registrada en Google Cloud.

## Flujo de usuario

Iniciar el flujo mediante:

```text
GET /api/v1/User/external/google
```

La API redirige al usuario a Google. Después de la autorización, Google devuelve el resultado a:

```text
GET {Authentication:PublicBaseUrl}{Authentication:Google:CallbackPath}
```

El middleware Google procesa esa ruta y redirige al endpoint de finalización de la aplicación:

```text
GET /api/v1/User/external/google/callback
```

La URL pública y la ruta técnica se combinan para construir el `redirect_uri` enviado a Google. La ruta se puede cambiar mediante `Authentication__Google__CallbackPath`, pero la URI resultante debe registrarse exactamente en Google Cloud.

Si la API está detrás de un proxy inverso, configurar `ForwardedHeaders__KnownProxies` con las IP reales de los proxies confiables. No se habilita confianza ilimitada en `X-Forwarded-*`.

El callback:

1. Comprueba que Google ha devuelto un correo verificado.
2. Busca primero el vínculo externo existente en `AspNetUserLogins`.
3. Si no existe, busca un usuario local por correo.
4. Si tampoco existe, crea un `User` local con el rol `USER`.
5. Vincula el login Google con el usuario local.
6. Emite el JWT local existente en la respuesta JSON.

El token no se incluye en la URL de redirección.

## Consideraciones de seguridad

- El Client Secret solo debe existir en variables de entorno o un gestor de secretos.
- Usar HTTPS fuera de un entorno local controlado.
- Registrar únicamente URI de redirección necesarias.
- No aceptar correos no verificados.
- La cuenta Google no recibe automáticamente permisos administrativos.
- Los roles y perfiles ecommerce continúan siendo responsabilidad de la aplicación.
- Si un usuario local ya tiene el mismo correo verificado, el login Google se vincula a ese perfil.

## Referencias

- [Google external login setup in ASP.NET Core](https://learn.microsoft.com/aspnet/core/security/authentication/social/google-logins)
- [External provider authentication in ASP.NET Core Identity](https://learn.microsoft.com/aspnet/core/security/authentication/social)
