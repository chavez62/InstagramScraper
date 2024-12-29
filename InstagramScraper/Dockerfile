FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
WORKDIR /src
COPY ["InstagramScraper.csproj", "./"]
RUN dotnet restore "InstagramScraper.csproj"
COPY . .
RUN dotnet build "InstagramScraper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InstagramScraper.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InstagramScraper.dll"]