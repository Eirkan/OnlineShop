#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Customer.Api/Customer.Api.csproj", "Customer.Api/"]
COPY ["Customer.Infrastructure/Customer.Infrastructure.csproj", "Customer.Infrastructure/"]
COPY ["Customer.Application/Customer.Application.csproj", "Customer.Application/"]
COPY ["Customer.Contracts/Customer.Contracts.csproj", "Customer.Contracts/"]
COPY ["Customer.Domain/Customer.Domain.csproj", "Customer.Domain/"]
COPY ["Customer.Core/Customer.Core.csproj", "Customer.Core/"]
RUN dotnet restore "Customer.Api/Customer.Api.csproj"
COPY . .
WORKDIR "/src/Customer.Api"
RUN dotnet build "Customer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Customer.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customer.Api.dll"]