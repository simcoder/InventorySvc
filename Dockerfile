FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY src/GOC.InventoryService.sln ./
COPY src/GOC.Inventory.Domain/GOC.Inventory.Domain.csproj src/GOC.Inventory.Domain
COPY src/GOC.Inventory.API/GOC.Inventory.API.csproj src/GOC.Inventory.API
COPY src/GOC.Inventory.Infrastructure/GOC.Inventory.Infrastructure.csproj src/GOC.Inventory.Infrastructure

RUN dotnet restore
COPY /src ./

WORKDIR /src/GOC.Inventory.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "GOC.Inventory.API.dll"]
