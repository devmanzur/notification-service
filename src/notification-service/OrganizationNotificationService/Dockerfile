FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["notification-service/OrganizationNotificationService/OrganizationNotificationService.csproj", "notification-service/OrganizationNotificationService/"]
RUN dotnet restore "notification-service/OrganizationNotificationService/OrganizationNotificationService.csproj"
COPY . .
WORKDIR "/src/notification-service/OrganizationNotificationService"
RUN dotnet build "OrganizationNotificationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrganizationNotificationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrganizationNotificationService.dll"]
