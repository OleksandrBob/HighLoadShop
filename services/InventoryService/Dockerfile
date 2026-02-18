# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY InventoryService.Api/InventoryService.Api.csproj InventoryService.Api/
COPY InventoryService.Application/InventoryService.Application.csproj InventoryService.Application/
COPY InventoryService.Domain/InventoryService.Domain.csproj InventoryService.Domain/
COPY InventoryService.Persistence/InventoryService.Persistence.csproj InventoryService.Persistence/

# Restore dependencies
RUN dotnet restore InventoryService.Api/InventoryService.Api.csproj

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR /src/InventoryService.Api
RUN dotnet build InventoryService.Api.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish InventoryService.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8082

# Copy published files
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8082
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "InventoryService.Api.dll"]
