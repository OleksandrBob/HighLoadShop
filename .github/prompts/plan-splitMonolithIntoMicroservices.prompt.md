# Plan: Split Monolith into Order and Inventory Microservices

This plan refactors the existing monolithic application into two independent microservices (Order Service and Inventory Service), each following Clean Architecture principles with separate Dockerfiles, databases, and docker-compose orchestration.

## Steps

1. **Create OrderService folder structure** - Create services/OrderService with four layers: OrderService.Api, OrderService.Application, OrderService.Domain, and OrderService.Persistence. Move Order-related code from HighLoadShop.Api/Controllers/OrdersController.cs, HighLoadShop.Domain/OrderContext, HighLoadShop.Application/OrderContext, and HighLoadShop.Persistence/OrderContext into respective layers. Copy HighLoadShop.Domain/Common and HighLoadShop.Application/Common to each service.

2. **Create InventoryService folder structure** - Create services/InventoryService with the same Clean Architecture structure. Move Inventory-related code from HighLoadShop.Api/Controllers/InventoryController.cs, HighLoadShop.Domain/InventoryContext, HighLoadShop.Application/InventoryContext, and HighLoadShop.Persistence/InventoryContext into respective layers.

3. **Create Dockerfiles for each service** - Create services/OrderService/Dockerfile and services/InventoryService/Dockerfile using multi-stage builds similar to the existing Dockerfile. Each should restore dependencies, build, publish, and use aspnet:10.0 runtime. Configure OrderService to expose port 8081 and InventoryService to expose port 8082.

4. **Update docker-compose.yml** - Modify docker-compose.yml to replace the single `api` service with `orderservice` and `inventoryservice`. Each service should have separate connection strings pointing to their respective databases (HighLoadShop_Orders and HighLoadShop_Inventory), separate build contexts (services/OrderService and services/InventoryService), and port mappings (5001:8081 for OrderService, 5002:8082 for InventoryService). Keep the existing `sqlserver` service with health checks.

5. **Implement inter-service communication** - Add HttpClient configuration to OrderService to call InventoryService's `/api/v1/inventory/reserve` endpoint when processing order creation. Update OrderService.Application/OrderContext/Commands/CreateOrder/CreateOrderCommandHandler.cs to make HTTP call to `http://inventoryservice:8082/api/v1/inventory/reserve` for inventory reservation instead of using `IInventoryRepository`. Add retry policies using Polly for resilience.

6. **Configure environment variables per service** - Update each service's Program.cs to read connection strings from environment variables. OrderService needs `ConnectionStrings__OrderDatabase`, InventoryService needs `ConnectionStrings__InventoryDatabase`. Add service discovery URLs (e.g., `InventoryService__BaseUrl`) for inter-service calls.

## Current Application Structure

```
HighLoadShop/
├── HighLoadShop.Api/                 # API Layer (Controllers, Models, Validators)
│   ├── Controllers/                  # OrdersController, InventoryController
│   ├── Models/                       # API request models
│   ├── Validators/                   # FluentValidation validators
│   └── Program.cs                    # Startup configuration
│
├── HighLoadShop.Application/         # Application Layer (CQRS handlers)
│   ├── Common/                       # Dispatcher, interfaces (ICommand, IQuery)
│   │   ├── Interfaces/
│   │   └── Models/                   # Result wrapper
│   ├── OrderContext/
│   │   ├── Commands/                 # CreateOrder, ConfirmOrder
│   │   ├── Queries/                  # GetOrder
│   │   └── Interfaces/               # IOrderRepository
│   └── InventoryContext/
│       ├── Commands/                 # ReserveInventory
│       ├── Queries/                  # GetInventory
│       └── Interfaces/               # IInventoryRepository
│
├── HighLoadShop.Domain/              # Domain Layer (Entities, Events)
│   ├── Common/                       # AggregateRoot, Entity, ValueObject, IDomainEvent
│   ├── OrderContext/
│   │   ├── Entities/                 # Order, OrderItem, OrderStatus
│   │   └── Events/                   # OrderConfirmed, Cancelled, Completed
│   ├── InventoryContext/
│   │   ├── Entities/                 # InventoryItem, Reservation
│   │   └── Events/                   # InventoryReserved, Released, Confirmed, StockAdded
│   ├── UserContext/                  # User entity (basic)
│   └── PaymentContext/               # Payment entity (basic)
│
├── HighLoadShop.Persistence/         # Data Access Layer
│   ├── OrderContext/
│   │   ├── OrderDbContext.cs         # Separate DB context
│   │   ├── Repositories/             # OrderRepository
│   │   └── Migrations/
│   └── InventoryContext/
│       ├── InventoryDbContext.cs     # Separate DB context
│       ├── Repositories/             # InventoryRepository
│       └── Migrations/
│
└── HighLoadShop.Infrastructure/      # Cross-cutting concerns
    └── DependencyInjection.cs        # Logging (AWS SQS placeholder)
```

## Target Microservices Structure

```
HighLoadShop/
├── services/
│   ├── OrderService/
│   │   ├── OrderService.Api/
│   │   │   ├── Controllers/          # OrdersController
│   │   │   ├── Models/
│   │   │   ├── Validators/
│   │   │   └── Program.cs
│   │   ├── OrderService.Application/
│   │   │   ├── Common/
│   │   │   ├── Commands/             # CreateOrder, ConfirmOrder
│   │   │   ├── Queries/              # GetOrder
│   │   │   └── Interfaces/
│   │   ├── OrderService.Domain/
│   │   │   ├── Common/
│   │   │   ├── Entities/             # Order, OrderItem
│   │   │   └── Events/
│   │   ├── OrderService.Persistence/
│   │   │   ├── OrderDbContext.cs
│   │   │   ├── Repositories/
│   │   │   └── Migrations/
│   │   ├── Dockerfile
│   │   └── OrderService.sln
│   │
│   └── InventoryService/
│       ├── InventoryService.Api/
│       │   ├── Controllers/          # InventoryController
│       │   ├── Models/
│       │   ├── Validators/
│       │   └── Program.cs
│       ├── InventoryService.Application/
│       │   ├── Common/
│       │   ├── Commands/             # ReserveInventory
│       │   ├── Queries/              # GetInventory
│       │   └── Interfaces/
│       ├── InventoryService.Domain/
│       │   ├── Common/
│       │   ├── Entities/             # InventoryItem, Reservation
│       │   └── Events/
│       ├── InventoryService.Persistence/
│       │   ├── InventoryDbContext.cs
│       │   ├── Repositories/
│       │   └── Migrations/
│       ├── Dockerfile
│       └── InventoryService.sln
│
├── docker-compose.yml                # Orchestrates both services + SQL Server
└── README.md
```

## Key Implementation Details

### OrderService Dependencies
- Microsoft.AspNetCore.OpenApi
- FluentValidation.DependencyInjectionExtensions
- Custom Dispatcher (for CQRS)
- Microsoft.EntityFrameworkCore.SqlServer
- HttpClient for calling InventoryService

### InventoryService Dependencies
- Microsoft.AspNetCore.OpenApi
- FluentValidation.DependencyInjectionExtensions
- Custom Dispatcher (for CQRS)
- Microsoft.EntityFrameworkCore.SqlServer

### Cross-Service Communication
- OrderService → InventoryService: HTTP POST to `/api/v1/inventory/reserve`
- Request model: `{ "orderId": "guid", "items": [{ "productId": "guid", "quantity": 10 }] }`
- Response: Success/Failure with available quantity information

### Database Configuration
- OrderService: `ConnectionStrings__OrderDatabase` → HighLoadShop_Orders
- InventoryService: `ConnectionStrings__InventoryDatabase` → HighLoadShop_Inventory
- Both services connect to same SQL Server instance (sqlserver container)

### Docker Compose Services
```yaml
services:
  sqlserver: # Existing SQL Server 2022
  orderservice: 
    - Build context: ./services/OrderService
    - Port: 5001:8081
    - Env: ConnectionStrings__OrderDatabase, InventoryService__BaseUrl
  inventoryservice:
    - Build context: ./services/InventoryService
    - Port: 5002:8082
    - Env: ConnectionStrings__InventoryDatabase
```

## Migration Strategy

1. **Phase 1**: Create new folder structure and copy code (keep original intact)
2. **Phase 2**: Create Dockerfiles and update docker-compose.yml
3. **Phase 3**: Test each service independently
4. **Phase 4**: Implement inter-service communication
5. **Phase 5**: Test end-to-end flow (Create Order → Reserve Inventory)
6. **Phase 6**: Remove old monolithic structure (optional - keep for comparison)

## Success Criteria

- ✅ OrderService runs independently on port 5001
- ✅ InventoryService runs independently on port 5002
- ✅ Both services connect to separate databases
- ✅ OrderService can call InventoryService via HTTP
- ✅ Creating an order automatically reserves inventory
- ✅ Each service follows Clean Architecture
- ✅ Each service has its own Dockerfile
- ✅ docker-compose up starts all services
