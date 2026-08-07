# Inventory integration API

## Purpose

This API exposes a small, read-only inventory surface from the ecommerce system of record for `IgnakeeAI.McpServer.Supplier`.

It is intended for service-to-service use by the Supplier MCP adapter so that external MCP clients can resolve prices, availability, and catalog synchronization data without gaining write access to ecommerce resources.

## Base path and versioning

- Base path: `/api/v1/inventory`
- Versioning style: URL versioning, consistent with the rest of the REST API
- Transport: HTTPS

## Authentication and authorization

- Authentication is required.
- The endpoint keeps the API's global authentication requirements in place.
- Access is restricted to identities with either the `INVENTORY_READER` role or the `ADMIN` role.

### Service-to-service expectation

The Supplier adapter should call this API using a technical identity that receives a bearer token from the existing authentication system and carries the `INVENTORY_READER` role claim. `ADMIN` is also accepted for operational break-glass access, but the preferred least-privilege assignment is `INVENTORY_READER`.

## Canonical identifier decision

`ProductCode` is the preferred canonical identifier because it is the explicit external product code already present in the product model.

Some existing products may not yet have a persisted `ProductCode`. For those records, the API exposes a deterministic fallback code in the form `product-{productId}`. This keeps the integration usable for existing catalog data while allowing future records to use their real `ProductCode` without changing the endpoint shape. Responses always include both `productCode` and `productId`.

## Endpoints

### Get a single product inventory record

`GET /api/v1/inventory/{productCode}`

Returns one active, externally exposed product.

#### 200 response example

```json
{
  "productCode": "SKU-IPHONE-001",
  "productId": 1,
  "productName": "IPhone",
  "description": "El iPhone dispone de cámara de fotos...",
  "category": "Tecnología",
  "price": 999.99,
  "currency": "USD",
  "isAvailableForSale": true,
  "stock": 1000,
  "unitToSell": "unit",
  "purchaseLeadTime": 2,
  "purchaseLeadTimeUnit": "day",
  "status": "Active"
}
```

#### Status code behavior

- `400` when `productCode` is missing or invalid
- `401` when the caller is not authenticated
- `403` when the caller is authenticated but lacks the required role
- `404` when the product does not exist, is inactive, obsolete, or otherwise not exposed by this API

### Search and paginate active catalog records

`GET /api/v1/inventory`

#### Query parameters

- `pageIndex` - 1-based page index, default `1`
- `pageSize` - page size, default `3`, max `50`
- `search` - optional text filter over product name, description, and explicit product code
- `categoryId` - optional category filter
- `sort` - optional sort using existing product conventions such as `nombreAsc`, `nombreDesc`, `precioAsc`, `precioDesc`

#### 200 response example

```json
{
  "count": 1,
  "pageIndex": 1,
  "pageSize": 10,
  "data": [
    {
      "productCode": "SKU-IPHONE-001",
      "productId": 1,
      "productName": "IPhone",
      "description": "El iPhone dispone de cámara de fotos...",
      "category": "Tecnología",
      "price": 999.99,
      "currency": "USD",
      "isAvailableForSale": true,
      "stock": 1000,
      "unitToSell": "unit",
      "purchaseLeadTime": 2,
      "purchaseLeadTimeUnit": "day",
      "status": "Active"
    }
  ],
  "pageCount": 1,
  "resultByPage": 1
}
```

#### Search semantics

- Only `ProductStatus.Active` products are returned.
- Inactive (`Desactive`) and obsolete (`Obsolete`) products are not exposed.
- `categoryId` follows the existing numeric category filter convention already used by product pagination.

## Compatibility notes

- The REST inventory API is separate from the existing `/api/mcp` endpoint.
- `/api/mcp` remains unchanged and continues to expose the current MCP tools.
- This REST contract is intended as the backend integration surface for the Supplier MCP adapter and can evolve independently through future API versions.
