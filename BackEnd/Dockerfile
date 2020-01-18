FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["cs309server/cs309server.csproj", "cs309server/"]
RUN dotnet restore "cs309server/cs309server.csproj"
COPY . .
WORKDIR "/src/cs309server"

RUN dotnet build "cs309server.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "cs309server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cs309server.dll"]
