
FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

# Copy csproj and restore as distinct layers
FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
#COPY NuGet.Config /src
COPY  CSAT.Services.Communication.Web.Core/CSAT.Services.Communication.Web.Core.csproj FTDNA.Services.GeneticInfo.Web.Core/
COPY  CSAT.Services.Communication.DataCore/CSAT.Services.Communication.DataCore.csproj  FTDNA.Services.GeneticInfo.DataCore/
COPY . .

# WORKDIR /src/FTDNA.Services.GeneticInfo.Web.Core
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

# Build runtime image
FROM base AS final
WORKDIR /app
COPY  --from=publish /app .
ENTRYPOINT ["dotnet", "CSAT.Services.Communication.Web.Core.dll"]
Psychiatry 