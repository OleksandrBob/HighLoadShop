---
description: "Use when working with the InventoryService in the HighLoadShop API."
applyTo: "services/InventoryService/**"
---

# InventoryService Guidelines

## Architecture & Infrastructure
- **Clean Architecture:** Divided into API, Application, Domain, and Persistence layers.
- **CQRS:** Handles write operations (Commands) and read operations (Queries) separately.
- **Entity Framework Core:** Used for tracking and managing inventory item availability.
- **Database:** Uses PostgreSQL for state persistence.

## Best Practices
- Keep domain entities isolated from external interactions.
- Ensure all Application logic is encapsulated in Command or Query Handlers (e.g., `ReserveInventory`).
- Setup Dependency Injection for each layer cleanly.
- Keep controllers thin by mapping requests directly to MediatR commands/queries.
