FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
ARG GITHUB_USERNAME
ARG GITHUB_TOKEN
WORKDIR /src
COPY src/ .

RUN dotnet nuget add source \
    --username $GITHUB_USERNAME \
    --password $GITHUB_TOKEN \
    --store-password-in-clear-text \
    --name github "https://nuget.pkg.github.com/nhanne/index.json"

RUN dotnet restore "ProductService.Api/ProductService.Api.csproj"

WORKDIR "/src/ProductService.Api"
RUN dotnet build "./ProductService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "ProductService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
COPY --from=publish /app/publish .
#COPY SSC.pfx /home
ENTRYPOINT ["dotnet", "ProductService.Api.dll"]