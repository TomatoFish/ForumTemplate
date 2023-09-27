FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/ThemeService/ThemeService.csproj", "src/Services/ThemeService/"]
RUN dotnet restore "src/Services/ThemeService/ThemeService.csproj"
COPY . .
WORKDIR "/src/src/Services/ThemeService"
RUN dotnet build "ThemeService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ThemeService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThemeService.dll"]
