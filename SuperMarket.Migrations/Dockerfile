FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SuperMarket.Migrations/SuperMarket.Migrations.csproj", "SuperMarket.Migrations/"]
RUN dotnet restore "SuperMarket.Migrations/SuperMarket.Migrations.csproj"
COPY . .
WORKDIR "/src/SuperMarket.Migrations"
RUN dotnet build "SuperMarket.Migrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SuperMarket.Migrations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SuperMarket.Migrations.dll"]
