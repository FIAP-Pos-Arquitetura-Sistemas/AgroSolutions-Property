# ----------------------------
# Estágio 1: Build
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 1. Copia o arquivo de projeto e restaura as dependências
# Usamos o wildcard para pegar o .csproj independente do nome exato
COPY *.csproj ./
RUN dotnet restore

# 2. Copia todo o restante do código fonte
COPY . ./

# 3. Publica a aplicação em modo Release
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ----------------------------
# Estágio 2: Runtime
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia os arquivos publicados do estágio de build
COPY --from=build /app/publish .

# No .NET 8 a porta padrão é 8080
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "AgroSolutions-FarmService.dll"]