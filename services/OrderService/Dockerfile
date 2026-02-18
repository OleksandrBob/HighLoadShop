# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY OrderService.Api/OrderService.Api.csproj OrderService.Api/
COPY OrderService.Application/OrderService.Application.csproj OrderService.Application/
COPY OrderService.Domain/OrderService.Domain.csproj OrderService.Domain/
COPY OrderService.Persistence/OrderService.Persistence.csproj OrderService.Persistence/

# Restore dependencies
RUN dotnet restore OrderService.Api/OrderService.Api.csproj

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR /src/OrderService.Api
RUN dotnet build OrderService.Api.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish OrderService.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8081

# Copy published files
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8081
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "OrderService.Api.dll"]
