FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:8000
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["CareGardenApiV1/CareGardenApiV1.csproj", "CareGardenApiV1/"]
RUN dotnet restore "CareGardenApiV1/CareGardenApiV1.csproj"

COPY . .
WORKDIR "/src/CareGardenApiV1"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CareGardenApiV1.dll"]