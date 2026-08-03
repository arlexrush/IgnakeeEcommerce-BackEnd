# Modernization Plan

## Overview

**Target**: Modernize the ecommerce backend for .NET 8 while preserving the existing layered architecture and preparing it for PostgreSQL, Stripe, Entra ID, MCP, and AI agent integration.
**Scope**: Domain, application, infrastructure, API, shipping workflow, and local developer experience for a modern ecommerce platform.

## Tasks

### 01-domain-model-and-architecture: Review and stabilize the core domain model

Audit the current domain model and application boundaries so the business rules for catalog, cart, checkout, orders, payments, and shipping remain consistent before introducing new infrastructure and integrations. The work should preserve the layered architecture and avoid a premature rewrite to microservices.

**Done when**: The key entities and relationships are documented, the shipping and order flow is aligned with the current use cases, and the architectural boundaries are clear for the next implementation phases.

### 02-persistence-and-local-dev: Replace SQL Server with PostgreSQL and Docker Compose

Switch the persistence layer from SQL Server to PostgreSQL, introduce containerized local development with Docker Compose and a persistent volume, and ensure EF Core migrations work in the new environment. The goal is to make the solution easy to run and consistent across environments.

**Done when**: The solution can run locally with PostgreSQL via Docker Compose, EF Core migrations apply successfully, and the API connects to PostgreSQL without SQL Server-specific assumptions.

### 03-authentication-and-identity: Introduce Entra ID for enterprise authentication

Replace the current Identity-based authentication approach with Entra ID / OpenID Connect for sign-in and token validation, while keeping ecommerce-specific user profile data in the application domain. The goal is to support enterprise-grade auth without breaking the existing user and address workflows.

**Done when**: The API authenticates users through Entra ID/OpenID Connect, token validation works in the .NET 8 pipeline, and the application can still manage user profile data needed by the ecommerce domain.

### 04-payments-and-webhooks: Integrate Stripe as the primary payments engine

Add Stripe Checkout or Payment Element as the main payment flow, persist the resulting payment intent and order state transitions, and process Stripe webhooks to keep the order lifecycle consistent. The implementation should use domain events or simple service orchestration rather than scattering payment logic across controllers.

**Done when**: Checkout creates a Stripe payment session or payment intent, webhooks update order/payment state correctly, and the solution handles payment success and failure in a predictable way.

### 05-shipping-and-carrier-adapters: Refine the shipping model and carrier integration layer

Refactor the shipping design so it is based on ports and adapters: one domain contract, one service layer, and carrier-specific implementations that can be extended without changing the core checkout flow. This should make the current shipping model easier to evolve and test.

**Done when**: Shipping is modeled as an explicit domain capability with adapter-based providers, the checkout flow can select a carrier strategy, and the model is ready for future webhook-based tracking.

### 06-mcp-integration: Expose application capabilities to MCP-compatible agents

Create a thin MCP integration layer over the existing application services so AI agents can interact with catalog, cart, order, and payment capabilities in a controlled way. The MCP surface should not bypass business rules or directly manipulate the persistence layer from the tooling side.

**Done when**: A documented MCP tool surface exists for core ecommerce operations, access is controlled, and the integration is driven through the application services rather than direct controller access.

### 07-ai-agents-and-observability: Prepare the platform for Microsoft Agent Framework and AI assistants

Introduce the foundation for Microsoft Agent Framework / Azure AI Foundry-oriented agent scenarios, including telemetry, request tracing, and a clear separation between orchestration and business logic. This stage should leave the backend ready for AI copilots or orchestrators without making the core domain dependent on them.

**Done when**: The solution has structured logging, traceability, and an architecture that supports AI-assisted workflows without coupling those agents directly to the domain model.
