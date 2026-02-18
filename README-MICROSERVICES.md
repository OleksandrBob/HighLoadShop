# HighLoadShop - Microservices Architecture

This application has been refactored into a microservices architecture with two independent services:
- **OrderService** - Handles order creation, confirmation, and management
- **InventoryService** - Manages product inventory and reservations

## Architecture Overview

```
┌─────────────────┐         ┌──────────────────────┐
│   OrderService  │────────>│  InventoryService    │
│   Port: 5001    │  HTTP   │    Port: 5002        │
└────────┬────────┘         └──────────┬───────────┘
         │                              │
         │                              │
         └──────────┬───────────────────┘
                    │
            ┌───────▼────────┐
            │   SQL Server   │
            │   Port: 1433   │
            │  ┌───────────┐ │
            │  │Orders  DB │ │
            │  │Inventory  │ │
            │  └───────────┘ │
            └────────────────┘
```

## Quick Start

### 1. Build and Run with Docker Compose

```bash
# Build and start all services
docker-compose up -d

# View logs
docker-compose logs -f

# View specific service logs
docker-compose logs -f orderservice
docker-compose logs -f inventoryservice
docker-compose logs -f sqlserver

# Stop services
docker-compose down
```

### 2. Service Endpoints

- **OrderService**: `http://localhost:5001`
  - POST `/api/v1/orders` - Create order
  - GET `/api/v1/orders/{orderId}` - Get order
  - POST `/api/v1/orders/{orderId}/confirm` - Confirm order

- **InventoryService**: `http://localhost:5002`
  - GET `/api/v1/inventory/{productId}` - Get inventory
  - POST `/api/v1/inventory/reserve` - Reserve inventory

- **SQL Server**: `localhost:1433`
  - Username: `sa`
  - Password: `YourStrong!Passw0rd`

### 3. Testing the Services

```bash
# Create an order (automatically reserves inventory)
curl -X POST http://localhost:5001/api/v1/orders \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "items": [
      {
        "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
        "quantity": 5
      }
    ]
  }'

# Get inventory status
curl http://localhost:5002/api/v1/inventory/3fa85f64-5717-4562-b3fc-2c963f66afa7

# Get order details
curl http://localhost:5001/api/v1/orders/{orderId}
```

## Project Structure

```
HighLoadShop/
├── services/
│   ├── OrderService/
│   │   ├── OrderService.Api/           # Web API layer
│   │   ├── OrderService.Application/   # Business logic & CQRS
│   │   ├── OrderService.Domain/        # Domain entities & events
│   │   ├── OrderService.Persistence/   # Data access & HTTP clients
│   │   └── Dockerfile
│   │
│   └── InventoryService/
│       ├── InventoryService.Api/
│       ├── InventoryService.Application/
│       ├── InventoryService.Domain/
│       ├── InventoryService.Persistence/
│       └── Dockerfile
│
├── docker-compose.yml
├── DOCKER.md
└── README.md
```

Each service follows **Clean Architecture** principles:
- **Domain Layer**: Core business logic, entities, and domain events
- **Application Layer**: Use cases, commands, queries, and interfaces
- **Persistence Layer**: Database access and external service clients
- **API Layer**: HTTP controllers, request/response models, and validation

## Inter-Service Communication

OrderService communicates with InventoryService via HTTP:

1. When an order is created, OrderService:
   - Creates the order in its database
   - Calls InventoryService's `/api/v1/inventory/reserve` endpoint
   - If reservation fails, cancels the order

2. Configuration in docker-compose.yml:
   ```yaml
   orderservice:
     environment:
       - InventoryService__BaseUrl=http://inventoryservice:8082
   ```

## Database Migrations

Both services use Entity Framework Core. To apply migrations:

```bash
# For OrderService
cd services/OrderService/OrderService.Api
dotnet ef migrations add InitialCreate --project ../OrderService.Persistence
dotnet ef database update --project ../OrderService.Persistence

# For InventoryService
cd services/InventoryService/InventoryService.Api
dotnet ef migrations add InitialCreate --project ../InventoryService.Persistence
dotnet ef database update --project ../InventoryService.Persistence
```

Or run migrations from within Docker containers:

```bash
# OrderService migrations
docker exec -it highloadshop-orderservice dotnet ef database update --project OrderService.Persistence.dll

# InventoryService migrations
docker exec -it highloadshop-inventoryservice dotnet ef database update --project InventoryService.Persistence.dll
```

## Development

### Run Services Locally (Without Docker)

1. **Start SQL Server** (via Docker):
   ```bash
   docker-compose up sqlserver -d
   ```

2. **Run OrderService**:
   ```bash
   cd services/OrderService/OrderService.Api
   dotnet run
   # Runs on http://localhost:8081
   ```

3. **Run InventoryService**:
   ```bash
   cd services/InventoryService/InventoryService.Api
   dotnet run
   # Runs on http://localhost:8082
   ```

4. **Update appsettings.Development.json** for local development:
   - OrderService needs `InventoryService:BaseUrl` = `http://localhost:8082`
   - Both services need connection strings pointing to `localhost,1433`

### Environment Variables

Each service reads configuration from:
1. `appsettings.json` (defaults)
2. `appsettings.{Environment}.json` (environment-specific)
3. **Environment variables** (highest priority)

**OrderService**:
- `ConnectionStrings__OrderDatabase` - Database connection
- `InventoryService__BaseUrl` - URL for InventoryService

**InventoryService**:
- `ConnectionStrings__InventoryDatabase` - Database connection

## Monitoring and Logs

```bash
# View all logs
docker-compose logs -f

# View specific service with timestamps
docker-compose logs -f --timestamps orderservice

# View last 100 lines
docker-compose logs --tail=100 inventoryservice

# Check service health
docker-compose ps
```

## Troubleshooting

### OrderService cannot connect to InventoryService
- Verify both containers are on the same network: `highloadshop-network`
- Check InventoryService is running: `docker ps`
- Verify URL configuration in OrderService environment variables

### Database connection errors
- Ensure SQL Server is healthy: `docker-compose ps sqlserver`
- Check connection strings include `TrustServerCertificate=True`
- Wait 30 seconds after SQL Server starts for full initialization

### Port conflicts
If ports 5001, 5002, or 1433 are already in use, change the port mappings in [docker-compose.yml](docker-compose.yml):
```yaml
ports:
  - "YOUR_PORT:8081"  # OrderService
  - "YOUR_PORT:8082"  # InventoryService
  - "YOUR_PORT:1433"  # SQL Server
```

## Clean Architecture Benefits

Each service is independently:
- **Deployable** - Deploy services separately
- **Scalable** - Scale OrderService and InventoryService independently
- **Testable** - Test each layer in isolation
- **Maintainable** - Changes in one service don't affect others
- **Technology-agnostic** - Can use different databases or technologies per service

## Future Enhancements

1. **API Gateway** - Add gateway (Ocelot/YARP) for unified entry point
2. **Event-Driven Communication** - Replace HTTP with message bus (RabbitMQ/Azure Service Bus)
3. **Service Discovery** - Add Consul or Eureka for dynamic service discovery
4. **Distributed Tracing** - Implement OpenTelemetry for request tracing
5. **Circuit Breaker** - Add Polly for resilience patterns
6. **Authentication** - Implement JWT-based auth across services
7. **Health Checks** - Add health check endpoints for each service
8. **API Versioning** - Implement versioning strategy
