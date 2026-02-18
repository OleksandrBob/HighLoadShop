# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

# Copy solution and project files
COPY HighLoadShop.slnx .
COPY HighLoadShop.Api/HighLoadShop.Api.csproj HighLoadShop.Api/
COPY HighLoadShop.Application/HighLoadShop.Application.csproj HighLoadShop.Application/
COPY HighLoadShop.Domain/HighLoadShop.Domain.csproj HighLoadShop.Domain/
COPY HighLoadShop.Infrastructure/HighLoadShop.Infrastructure.csproj HighLoadShop.Infrastructure/
COPY HighLoadShop.Persistence/HighLoadShop.Persistence.csproj HighLoadShop.Persistence/

# Restore dependencies
RUN dotnet restore HighLoadShop.Api/HighLoadShop.Api.csproj

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR /src/HighLoadShop.Api
RUN dotnet build HighLoadShop.Api.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish HighLoadShop.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published files
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "HighLoadShop.Api.dll"]
