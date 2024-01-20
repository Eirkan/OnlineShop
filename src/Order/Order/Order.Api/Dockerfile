#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Order/Order.Api/Order.Api.csproj", "Order/Order.Api/"]
COPY ["Order/Order.Infrastructure/Order.Infrastructure.csproj", "Order/Order.Infrastructure/"]
COPY ["Order/Order.Application/Order.Application.csproj", "Order/Order.Application/"]
COPY ["Order/Order.Contracts/Order.Contracts.csproj", "Order/Order.Contracts/"]
COPY ["Order/Order.Core/Order.Core.csproj", "Order/Order.Core/"]
COPY ["Order/Order.Core.IntegrationEventLogEF/Order.Core.IntegrationEventLogEF.csproj", "Order/Order.Core.IntegrationEventLogEF/"]
COPY ["Order/Order.Domain/Order.Domain.csproj", "Order/Order.Domain/"]
COPY ["Order/Order.Core.EventBusRabbitMQ/Order.Core.EventBusRabbitMQ.csproj", "Order/Order.Core.EventBusRabbitMQ/"]
RUN dotnet restore "./Order/Order.Api/./Order.Api.csproj"
COPY . .
WORKDIR "/src/Order/Order.Api"
RUN dotnet build "./Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Api.dll"]