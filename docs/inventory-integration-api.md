# Inventory Integration API

This document describes the read-only HTTP API that the **IgnakeeAI.McpServer.Supplier** service-to-service adapter consumes to expose `GetPrice`, `CheckAvailability`, and catalog-synchronisation capabilities to external MCP clients.

---

## Overview

The Inventory Integration API sits under the existing `api/v1` routing convention and delegates all data access through MediatR query handlers in `Ecommerce.Application`. No business logic lives in the controller, and no write/mutation capability is exposed.

```
MCP Client / AI Agent
        ↓  (MCP protocol)
IgnakeeAI.McpServer.Supplier
        ↓  (HTTP – ****** SUPPLIER_INTEGRATION or ADMIN role)
GET /api/v1/inventory/product/{productCode}
GET /api/v1/inventory/catalog
        ↓
Ecommerce.Application (MediatR query handlers)
        ↓
PostgreSQL via EcommerceDbContext
```

---

## Authentication & Authorization

### Scheme
All endpoints require a valid ******** issued by this application's local JWT authority (same `JwtSettings:Key` used by the rest of the API).

### Policy
```
Policy name: SupplierIntegration
Requires: authenticated user AND (role = ADMIN OR role = SUPPLIER_INTEGRATION)
```

The `SUPPLIER_INTEGRATION` role is a least-privilege service identity intended exclusively for the MCP Supplier adapter. It has **no access** to order, user, payment, or admin endpoints.

### Setting up a service account
1. Register a user (or use an existing technical user) in the ecommerce Identity store.
2. Assign the `SUPPLIER_INTEGRATION` role via the admin role-assignment endpoint.
3. Generate a JWT for that user and configure it as the `Bearer` token in the MCP Supplier adapter.

> **Important:** Do not reuse end-user credentials. The service-to-service token should be rotated on your chosen schedule and kept in a secrets manager (not in source code or config files).

### HTTP status codes

| Condition | Status |
|---|---|
| No `Authorization` header or invalid/expired token | `401 Unauthorized` |
| Valid token but missing `ADMIN` or `SUPPLIER_INTEGRATION` role | `403 Forbidden` |
| Invalid input (e.g. blank `productCode`) | `400 Bad Request` |
| Product not found or not active | `404 Not Found` |
| Successful response | `200 OK` |

---

## Canonical Product Identifier

**`ProductCode`** is the canonical external identifier used throughout this API. It is a business-assigned code (e.g. `"P-001"`) stored in `Product.ProductCode` and is the natural key for integration. The numeric `productId` (`Product.Id`) is also returned in responses for internal correlation but **should not** be treated as the stable public key.

---

## Endpoints

### 1. Get product inventory by code

```
GET /api/v1/inventory/product/{productCode}
Authorization: ******
```

Returns the inventory view for a single **Active** product.  
**`404 Not Found`** is returned for products that do not exist or are not in the `Active` state (Desactive/inactive and Obsolete products are not exposed).

#### Path parameter

| Parameter | Type | Description |
|---|---|---|
| `productCode` | string | Canonical product code (e.g. `P-001`). Case-sensitive. |

#### Success response – `200 OK`

```json
{
  "productCode": "P-001",
  "productId": 42,
  "productName": "Organic Ground Coffee",
  "description": "Single-origin, medium roast, 250 g.",
  "category": "Beverages",
  "price": 12.50,
  "currency": "EUR",
  "stock": 35,
  "unitToSell": "bag",
  "purchaseLeadTime": 3,
  "purchaseLeadTimeUnit": "days",
  "status": 0
}
```

`status` values: `0 = Active`, `1 = Desactive` (inactive), `2 = Obsolete`. Note: `Desactive` is the domain enum name in this codebase; it means the product is not actively for sale.

#### Error responses

```
400 Bad Request   – productCode is blank
404 Not Found     – product does not exist or is not Active
```

---

### 2. Get active product catalog (paginated)

```
GET /api/v1/inventory/catalog
Authorization: ******
```

Returns a paginated list of **Active** products suitable for full or incremental catalog synchronization. All filters are optional.

#### Query parameters

| Parameter | Type | Default | Description |
|---|---|---|---|
| `pageIndex` | int | `1` | 1-based page number |
| `pageSize` | int | `3` | Items per page (max `50`) |
| `search` | string | — | Free-text search on `ProductName` or `Description` |
| `sort` | string | `createdDesc` | Sort key: `nombreAsc`, `nombreDesc`, `precioAsc`, `precioDesc`, `ratingAsc`, `ratingDesc` |
| `categoryId` | int | — | Filter by category primary key |

#### Success response – `200 OK`

```json
{
  "count": 87,
  "pageIndex": 1,
  "pageSize": 10,
  "pageCount": 9,
  "resultByPage": 9,
  "data": [
    {
      "productCode": "P-001",
      "productId": 42,
      "productName": "Organic Ground Coffee",
      "description": "Single-origin, medium roast, 250 g.",
      "category": "Beverages",
      "price": 12.50,
      "currency": "EUR",
      "stock": 35,
      "unitToSell": "bag",
      "purchaseLeadTime": 3,
      "purchaseLeadTimeUnit": "days",
      "status": 0
    }
  ]
}
```

- Only `Active` products are returned (this is enforced server-side; the `status` field in the response will always be `0` for this endpoint).
- The paginated envelope is the same `PaginationVm<T>` shape used across the rest of the API.
- For incremental synchronization, iterate pages until `pageIndex >= pageCount`.

---

## Integration Notes for IgnakeeAI.McpServer.Supplier

### Recommended approach (hybrid)
- **Catalog / prices**: synchronize periodically via `GET /api/v1/inventory/catalog` (all pages). Cache locally for low-latency MCP `GetPrice` and `SearchAlternatives` calls.
- **Stock / availability**: query `GET /api/v1/inventory/product/{productCode}` in real time for `CheckAvailability`, as stock can change with each order.

### Null field semantics
- `stock: null` means stock is not tracked for this product (treat as available).
- `stock: 0` means explicitly out of stock.
- `price: null` / `currency: null` means pricing is not configured (return an appropriate "price unavailable" response from the MCP tool).

---

## Versioning & Compatibility

- The API follows the existing `api/v1/` prefix convention of this application.
- Non-breaking additions (new optional response fields) will not require a version bump.
- Breaking changes will be introduced under `api/v2/`.
- The `/api/mcp` MCP server surface is a **separate endpoint** and is not replaced by this REST API.
