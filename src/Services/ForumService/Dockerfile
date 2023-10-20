FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/ForumService/ForumService.csproj", "src/Services/ForumService/"]
RUN dotnet restore "src/Services/ForumService/ForumService.csproj"
COPY . .
WORKDIR "/src/src/Services/ForumService"
RUN dotnet build "ForumService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ForumService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ForumService.dll"]
