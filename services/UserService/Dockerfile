# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY UserService.Api/UserService.Api.csproj UserService.Api/
COPY UserService.Application/UserService.Application.csproj UserService.Application/
COPY UserService.Domain/UserService.Domain.csproj UserService.Domain/
COPY UserService.Persistence/UserService.Persistence.csproj UserService.Persistence/

# Restore dependencies
RUN dotnet restore UserService.Api/UserService.Api.csproj

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR /src/UserService.Api
RUN dotnet build UserService.Api.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish UserService.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8083

# Copy published files
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8083
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "UserService.Api.dll"]