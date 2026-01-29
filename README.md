# HighLoadShop Clean Architecture Implementation

## Overview
This project demonstrates a clean architecture implementation for a high-load e-commerce shop with the following bounded contexts:
- **Order Context**: Order management and processing
- **Inventory Context**: Stock management and reservations  
- **User Context**: User management (basic structure)
- **Payment Context**: Payment processing (basic structure)

## Architecture

### Project Structure
- **HighLoadShop.Domain**: Core business entities, value objects, and domain events
- **HighLoadShop.Application**: Use cases, commands, queries, and DTOs (CQRS ready)
- **HighLoadShop.Persistence**: EF Core contexts, repositories (separate database per context)
- **HighLoadShop.Infrastructure**: Cross-cutting concerns, dependency injection
- **HighLoadShopApi**: Minimal API endpoints with v1 versioning

### Key Features
✅ Clean Architecture with proper dependency inversion
✅ Bounded contexts prepared for microservices migration
✅ Separate databases per context (Order & Inventory)
✅ Domain events for inter-context communication
✅ CQRS pattern with MediatR
✅ EF Core + prepared for Dapper integration
✅ Minimal API with v1 versioning
✅ Optimistic concurrency support in entities
✅ Repository pattern implementation

## API Endpoints

### Orders (Order Context)
- `POST /api/v1/orders` - Create new order
- `GET /api/v1/orders/{orderId}` - Get order details
- `POST /api/v1/orders/{orderId}/confirm` - Confirm order

### Inventory (Inventory Context)  
- `GET /api/v1/inventory/{productId}` - Get inventory status
- `POST /api/v1/inventory/reserve` - Reserve inventory for order

## Database Setup
The application uses separate databases for each bounded context:
- `HighLoadShop_Orders` - Order management
- `HighLoadShop_Inventory` - Inventory management

## Running the Application
1. Ensure SQL Server LocalDB is available
2. Run `dotnet ef database update --context OrderDbContext` 
3. Run `dotnet ef database update --context InventoryDbContext`
4. Start API: `dotnet run --project HighLoadShopApi`
5. API available at: http://localhost:5111
6. OpenAPI/Swagger: http://localhost:5111/openapi/v1.json

## Next Steps for Production
- [ ] Add User Context full implementation
- [ ] Add Payment Context full implementation  
- [ ] Implement Dapper for read-heavy operations
- [ ] Add domain event handlers
- [ ] Add AWS SQS integration in Infrastructure
- [ ] Add comprehensive logging and monitoring
- [ ] Add authentication & authorization
- [ ] Add comprehensive validation with FluentValidation
- [ ] Add integration tests
- [ ] Add API rate limiting
- [ ] Add health checks