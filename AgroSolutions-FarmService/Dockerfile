# ----------------------------
# Estágio 1: Build
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Criando uma pasta interna no Docker para o build
WORKDIR /home/app 

# 1. Copia o arquivo de projeto
COPY ["AgroSolutions-FarmService.csproj", "./"]
RUN dotnet restore "AgroSolutions-FarmService.csproj"

# 2. Copia todo o restante dos arquivos para o Docker
COPY . .

# 3. Publica a aplicação
RUN dotnet publish "AgroSolutions-FarmService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ----------------------------
# Estágio 2: Runtime
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "AgroSolutions-FarmService.dll"]
