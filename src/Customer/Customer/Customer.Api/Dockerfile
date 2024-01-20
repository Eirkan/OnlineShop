#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Customer/Customer.Api/Customer.Api.csproj", "Customer/Customer.Api/"]
COPY ["Customer/Customer.Infrastructure/Customer.Infrastructure.csproj", "Customer/Customer.Infrastructure/"]
COPY ["Customer/Customer.Application/Customer.Application.csproj", "Customer/Customer.Application/"]
COPY ["Customer/Customer.Contracts/Customer.Contracts.csproj", "Customer/Customer.Contracts/"]
COPY ["Customer/Customer.Core/Customer.Core.csproj", "Customer/Customer.Core/"]
COPY ["Customer/Customer.Core.IntegrationEventLogEF/Customer.Core.IntegrationEventLogEF.csproj", "Customer/Customer.Core.IntegrationEventLogEF/"]
COPY ["Customer/Customer.Domain/Customer.Domain.csproj", "Customer/Customer.Domain/"]
COPY ["Customer/Customer.Core.EventBusRabbitMQ/Customer.Core.EventBusRabbitMQ.csproj", "Customer/Customer.Core.EventBusRabbitMQ/"]
RUN dotnet restore "./Customer/Customer.Api/./Customer.Api.csproj"
COPY . .
WORKDIR "/src/Customer/Customer.Api"
RUN dotnet build "./Customer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Customer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customer.Api.dll"]