# Gateway con YARP

## Propósito

`Ecommerce.Gateway` es el punto de entrada HTTP de los servicios de ecommerce. Es un proxy inverso ligero basado en YARP: enruta solicitudes al backend sin contener lógica de negocio, persistencia ni autenticación propia.

## Rutas y destinos

La configuración `ReverseProxy` define inicialmente un único cluster:

- Cualquier ruta (`{**catch-all}`) se reenvía a `Ecommerce.Api`.
- En contenedores, el destino predeterminado es `http://api:8080/`.
- Al ejecutar los proyectos localmente, `appsettings.Development.json` utiliza `http://localhost:5204/`, que es el perfil HTTP actual de la API.
- `GET /health` se resuelve en el gateway y no se reenvía.

Para cambiar el destino sin modificar archivos se puede establecer la variable de entorno `ReverseProxy__Clusters__api-cluster__Destinations__api__Address`.

## Autenticación y seguridad

YARP reenvía de forma nativa el encabezado `Authorization`; por tanto, los JWT locales y de Microsoft Entra llegan sin modificación a la API. La API mantiene la validación de tokens y la autorización de cada endpoint. El gateway no debe recibir claves, secretos ni implementar una segunda validación de credenciales.

En producción, el gateway es el único servicio HTTP publicado por Compose. La API no publica puertos y solo se comunica dentro de la red de Docker. Si TLS termina en un proxy externo, configure los proxies de confianza de la API mediante `ForwardedHeaders__KnownProxies` según la infraestructura desplegada.

## Observabilidad

El gateway escribe los eventos de enrutamiento de YARP mediante `ILogger` con la categoría `Yarp` a nivel `Information`. El endpoint `GET /health` permite a Docker, un balanceador o un monitor comprobar que el proceso acepta solicitudes.

## Ejecución

Para desarrollo con los proyectos ejecutados fuera de Docker:

1. Inicie `Ecommerce.Api` en el perfil HTTP configurado en el puerto 5204.
2. Ejecute `dotnet run --project src/Gateway/Ecommerce.Gateway/Ecommerce.Gateway.csproj`.
3. Use `http://localhost:5000` como punto de entrada.

Para el entorno local en contenedores:

1. Defina `GATEWAY_PORT` en `.env` si necesita otro puerto; el valor predeterminado es `5000`.
2. Ejecute `docker compose up --build`.
3. Use `http://localhost:5000` o el valor de `GATEWAY_PORT`.

En producción, `docker-compose.production.yml` expone `GATEWAY_PORT` (8080 por defecto). La terminación TLS y las reglas de firewall externas deben dirigirse exclusivamente a ese puerto del gateway.
