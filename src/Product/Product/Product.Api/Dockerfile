#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Product/Product.Api/Product.Api.csproj", "Product/Product.Api/"]
COPY ["Product/Product.Infrastructure/Product.Infrastructure.csproj", "Product/Product.Infrastructure/"]
COPY ["Product/Product.Application/Product.Application.csproj", "Product/Product.Application/"]
COPY ["Product/Product.Contracts/Product.Contracts.csproj", "Product/Product.Contracts/"]
COPY ["Product/Product.Core/Product.Core.csproj", "Product/Product.Core/"]
COPY ["Product/Product.Core.IntegrationEventLogEF/Product.Core.IntegrationEventLogEF.csproj", "Product/Product.Core.IntegrationEventLogEF/"]
COPY ["Product/Product.Domain/Product.Domain.csproj", "Product/Product.Domain/"]
COPY ["Product/Product.Core.EventBusRabbitMQ/Product.Core.EventBusRabbitMQ.csproj", "Product/Product.Core.EventBusRabbitMQ/"]
RUN dotnet restore "./Product/Product.Api/./Product.Api.csproj"
COPY . .
WORKDIR "/src/Product/Product.Api"
RUN dotnet build "./Product.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Product.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.Api.dll"]