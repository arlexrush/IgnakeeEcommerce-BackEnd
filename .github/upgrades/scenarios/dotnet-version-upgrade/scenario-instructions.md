# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net8.0

## Source Control
- **Source Branch**: develop
- **Working Branch**: upgrade-dotnet-8
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: All-at-Once

### Compatibility
- Unsupported Packages: Resolve Inline
- Unsupported API Handling: Fix Inline

## Decisions
- Preserve the existing layered architecture and avoid a rewrite to microservices; introduce ports/adapters and domain services instead.
- Review the shipping and checkout domain model before implementation so the persistence model supports multi-carrier, webhook-driven workflows.
- Replace SQL Server with PostgreSQL via Docker Compose and a persistent volume for local development and future deployment.
- Replace ASP.NET Core Identity with Entra ID/OpenID Connect for enterprise authentication while keeping a local user profile store for ecommerce-specific data.
- Integrate Stripe as the primary payment engine using Checkout Sessions and webhook-driven order/payment state transitions.
- Add MCP integration as a thin tool layer over the application services rather than coupling agents directly to the API controllers.
- Evaluate Microsoft Agent Framework / Azure AI Foundry as the orchestration layer for AI assistants, but keep the business logic inside the .NET backend.
- Use phased delivery: data model and persistence first, then identity/payments/shipping, then MCP/AI integration.

## User Preferences
### Execution Style
- **Pace**: Methodical
- **Risk Tolerance**: Conservative

### Technical Preferences
- **Architecture**: Keep a modular monolith; avoid microservices for now.
- **Simplicity**: Keep the code simple, documented, easy to maintain, and testable.
- **Environments**: Use two environments: develop and production.
- **Deployment**: Target Hetzner for both environments.
- **Persistence**: Use PostgreSQL with Docker Compose and a persistent volume.
- **Messaging**: Add RabbitMQ as the asynchronous messaging backbone for integration events.
- **MCP**: Expose MCP capabilities through a thin API endpoint on the existing backend.
- **AI Orchestration**: Separate agent orchestration from model orchestration; use Azure AI Foundry as the entry point and Azure OpenAI or DeepSeek-compatible models behind a simple provider abstraction.
- **Payments**: Use Stripe as the primary payment engine with webhook-driven state updates.
- **Authentication**: Use Entra ID / OpenID Connect for enterprise authentication and keep a local profile store for ecommerce-specific data.
