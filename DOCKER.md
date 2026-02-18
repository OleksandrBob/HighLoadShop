# HighLoadShop - Docker Setup

This guide explains how to run the HighLoadShop application using Docker.

## Prerequisites

- Docker Desktop installed and running
- Docker Compose (included with Docker Desktop)

## Quick Start

### 1. Build and Run with Docker Compose

```bash
# Build and start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

The application will be available at: `http://localhost:5000`

### 2. Access SQL Server

SQL Server will be available on `localhost:1433` with these credentials:
- **Username**: `sa`
- **Password**: `YourStrong!Passw0rd`

### 3. Run Database Migrations

After the containers are running, you need to apply database migrations:

```bash
# Connect to the API container
docker exec -it highloadshop-api bash

# Run migrations for Order Database
dotnet ef database update --context OrderDbContext

# Run migrations for Inventory Database
dotnet ef database update --context InventoryDbContext

# Exit the container
exit
```

Alternatively, run migrations from your host machine by updating the connection string temporarily:

```bash
# From the HighLoadShop.Api directory
dotnet ef database update --context OrderDbContext --project ../HighLoadShop.Persistence -- --ConnectionStrings:OrderDatabase="Data Source=localhost,1433;Initial Catalog=HighLoadShop_Orders;User ID=sa;Password=YourStrong!Passw0rd;Encrypt=True;TrustServerCertificate=True"
```

## Configuration

### Environment Variables

The application reads configuration from environment variables. In Docker Compose, these are set in the `docker-compose.yml` file.

Key environment variables:
- `ConnectionStrings__OrderDatabase` - Connection string for Orders database
- `ConnectionStrings__InventoryDatabase` - Connection string for Inventory database
- `ASPNETCORE_ENVIRONMENT` - Application environment (Development/Production)
- `ASPNETCORE_URLS` - URLs the application listens on

**Note**: Use double underscores (`__`) to represent nested configuration in environment variables.

### Custom Configuration

1. Copy the example environment file:
   ```bash
   cp .env.example .env
   ```

2. Edit `.env` with your custom values

3. Update `docker-compose.yml` to use the `.env` file:
   ```yaml
   env_file:
     - .env
   ```

## Docker Commands

### Build Only
```bash
docker-compose build
```

### Start Services
```bash
docker-compose up -d
```

### View Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f api
docker-compose logs -f sqlserver
```

### Stop Services
```bash
docker-compose down
```

### Stop and Remove Volumes (⚠️ Deletes all data)
```bash
docker-compose down -v
```

### Rebuild from Scratch
```bash
docker-compose down
docker-compose build --no-cache
docker-compose up -d
```

## Production Deployment

For production:

1. **Change the SQL Server password** in `docker-compose.yml` and connection strings
2. **Use secrets management** instead of hardcoded passwords
3. **Configure proper volumes** for persistent data
4. **Set up reverse proxy** (nginx/traefik) for HTTPS
5. **Enable health checks** and monitoring
6. **Review security settings** in Dockerfile

## Troubleshooting

### SQL Server not starting
- Ensure password meets SQL Server complexity requirements (8+ chars, uppercase, lowercase, number, special char)
- Check if port 1433 is already in use: `netstat -an | findstr 1433`
- View SQL Server logs: `docker-compose logs sqlserver`

### API cannot connect to SQL Server
- Wait for SQL Server to be fully ready (~30 seconds on first start)
- Check health status: `docker-compose ps`
- Verify connection strings in docker-compose.yml
- Ensure databases are created and migrations are applied

### Port conflicts
If port 5000 or 1433 is already in use, change the port mapping in `docker-compose.yml`:
```yaml
ports:
  - "YOUR_PORT:8080"  # For API
  - "YOUR_PORT:1433"  # For SQL Server
```

## Development vs Production

### Development
- Use `appsettings.Development.json`
- Run locally without Docker for faster iteration
- Connect to SQL Server in Docker: `localhost:1433`

### Production
- Use Docker Compose for all services
- Environment variables override appsettings
- Enable HTTPS and proper authentication
- Use managed databases for better reliability

## File Structure

```
HighLoadShop/
├── Dockerfile                 # Multi-stage Docker build
├── docker-compose.yml         # Service orchestration
├── .dockerignore             # Files to exclude from Docker build
├── .env.example              # Environment variables template
└── DOCKER.md                 # This file
```
