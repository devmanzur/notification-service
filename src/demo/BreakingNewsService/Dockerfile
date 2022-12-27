FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["demo/BreakingNewsService/BreakingNewsService.csproj", "demo/BreakingNewsService/"]
COPY ["notification-plugin/OrganizationNotificationPlugin/OrganizationNotificationPlugin.csproj", "notification-plugin/OrganizationNotificationPlugin/"]
COPY ["shared/BreakingNews.Messages/BreakingNews.Messages.csproj", "shared/BreakingNews.Messages/"]
RUN dotnet restore "demo/BreakingNewsService/BreakingNewsService.csproj"
COPY . .
WORKDIR "/src/demo/BreakingNewsService"
RUN dotnet build "BreakingNewsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BreakingNewsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BreakingNewsService.dll"]
