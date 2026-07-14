---
description: "Use when working with the OrderService in the HighLoadShop API."
applyTo: "services/OrderService/**"
---

# OrderService Guidelines

## Architecture & Infrastructure
- **Clean Architecture:** Organized strictly into API, Application, Domain, and Infrastructure/Persistence layers.
- **CQRS:** Uses the Command Query Responsibility Segregation pattern for all application operations.
- **Entity Framework Core:** Used for database access in the Persistence layer.
- **Database:** Uses PostgreSQL for order data storage.

## Best Practices
- Keep domain entities isolated from external concerns.
- Ensure all Application logic is encapsulated in Command or Query Handlers.
- Leverage Dependency Injection for all service layers (API, Application, Infrastructure, Persistence).
- Use asynchronous operations (`async`/`await`) for all cross-boundary tasks.
