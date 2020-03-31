#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM microsoft/aspnetcore:2.0-stretch AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-stretch AS build
WORKDIR /src
COPY ["CSAT.Services.Communication.Web.Core/CSAT.Services.Communication.Web.Core.csproj", "CSAT.Services.Communication.Web.Core/"]
COPY ["CSAT.Services.Communication.DataCore/CSAT.Services.Communication.DataCore.csproj", "CSAT.Services.Communication.DataCore/"]
RUN dotnet restore "CSAT.Services.Communication.Web.Core/CSAT.Services.Communication.Web.Core.csproj"
COPY . .
WORKDIR "/src/CSAT.Services.Communication.Web.Core"
RUN dotnet build "CSAT.Services.Communication.Web.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CSAT.Services.Communication.Web.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CSAT.Services.Communication.Web.Core.dll"]