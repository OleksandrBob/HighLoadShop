# Plan: Clean Architecture Multi-Project Solution with Bounded Contexts

Transform the existing single-project solution into a comprehensive clean architecture implementation with 5 separate projects, supporting Order, Inventory, Payment, and User bounded contexts using EF Core + Dapper with SQL Server, designed for future microservices migration.

## Steps

1. **Create solution structure** - Add Domain, Application, Persistence, Infrastructure projects to HighLoadShop.slnx and establish project dependencies to make sure that project folows clean architecture
2. **Implement Domain layer** - Create entities, aggregates, and value objects for Order, Inventory, Payment, User contexts in Domain project with proper bounded context separation
3. **Build Application layer** - Add commands/queries, DTOs, and application services for each bounded context. Make sure that later on sqrs pattern may be used
4. **Setup Persistence layer** - Configure EF Core DbContexts, Dapper repositories, and SQL Server integration with optimistic concurrency and proper table structures
5. **Configure Infrastructure layer** - Implement cross-cutting concerns, dependency injection, and prepare AWS SQS integration foundation
6. **Update API layer** - Restructure HighLoadShopApi/Program.cs to use minimal API endpoints with clean architecture wiring and bounded context routing. Make sure that all endpoints have `v1` prefix, as in the future some endpoints may be in the `v2`

## Further Considerations

1. **Database schema approach** -  we should use separate databases per bounded context. Eacho context must have a separate ef core DB context
2. **Inter-context communication** - Domain events for loose coupling between Order/Inventory contexts shoild be used

## Required Database Tables

### Order Context
- **Order**
  - OrderId
  - UserId
  - OrderItems[]
  - Status
  - ReservedUntil (UTC)
  - CreatedAt
  
- **OrderItem**
  - ProductId
  - Quantity

### Inventory Context
- **InventoryItem**
  - ProductId
  - TotalQuantity
  - ReservedQuantity
  
- **Reservation**
  - ReservationId
  - OrderId
  - Quantity
  - ExpiresAt

## Architecture Goals

- Clean separation of concerns with clear bounded contexts
- Easy migration path to microservices
- Hybrid EF Core + Dapper approach for performance
- Minimal API implementation
- SQL Server backend
- Future AWS SQS integration support
