#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["azureplaywebapi/azureplaywebapi.csproj", "azureplaywebapi/"]
RUN dotnet restore "azureplaywebapi/azureplaywebapi.csproj"
COPY . .
WORKDIR "/src/azureplaywebapi"
RUN dotnet build "azureplaywebapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "azureplaywebapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "azureplaywebapi.dll"]