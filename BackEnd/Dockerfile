FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SelecTunes-Backend/SelecTunes-Backend.csproj", "SelecTunes-Backend/"]
RUN dotnet restore "SelecTunes-Backend/SelecTunes-Backend.csproj"
COPY . .
WORKDIR "/src/SelecTunes-Backend"

RUN dotnet build "SelecTunes-Backend.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "SelecTunes-Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SelecTunes-Backend.dll"]
