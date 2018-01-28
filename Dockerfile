FROM microsoft/dotnet:2.0-sdk

# Separate dotnet restore to take advantage of cached builds


WORKDIR /app
COPY src/GOC.InventoryService ./
RUN dotnet restore && \dotnet build && \dotnet publish -o out

ENTRYPOINT ["dotnet", "/app/out/GOC.InventoryService.dll"]