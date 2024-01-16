#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:44300;http://+:8080;
ENV ASPNETCORE_ENVIRONMENT=Development


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RestWithASPNET.Api/RestWithASPNET.Api.csproj", "RestWithASPNET.Api/"]
RUN dotnet restore "./RestWithASPNET.Api/./RestWithASPNET.Api.csproj"
COPY . .
WORKDIR "/src/RestWithASPNET.Api"
RUN dotnet build "./RestWithASPNET.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RestWithASPNET.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestWithASPNET.Api.dll"]