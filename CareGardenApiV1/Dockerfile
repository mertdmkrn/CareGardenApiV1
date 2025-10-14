# 1. Base image olarak .NET 9 SDK kullanıyoruz
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Çalışma dizinini ayarla
WORKDIR /app

# csproj dosyalarını kopyala ve restore et
COPY *.csproj ./
RUN dotnet restore

# Tüm proje dosyalarını kopyala ve publish et
COPY . ./
RUN dotnet publish -c Release -o out

# 2. Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "CareGardenApiV1.dll"]