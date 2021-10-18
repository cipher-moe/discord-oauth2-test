FROM mcr.microsoft.com/dotnet/aspnet:5.0.7-alpine3.13-amd64 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0.301-alpine3.13-amd64 AS build
WORKDIR /src
COPY ["discord-oauth-test.csproj", "./"]
COPY ["nuget.config", "./"]
RUN dotnet restore "discord-oauth-test.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "discord-oauth-test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "discord-oauth-test.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "discord-oauth-test.dll"]