FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY api/api.csproj api/
RUN dotnet restore api/api.csproj

COPY api/ api/
RUN dotnet publish api/api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "api.dll"]
