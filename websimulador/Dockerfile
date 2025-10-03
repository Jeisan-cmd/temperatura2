# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos solo el csproj primero para aprovechar cache
COPY ["websimulador.csproj", "."]
RUN dotnet restore "websimulador.csproj"

# Copiamos todo el proyecto y publicamos
COPY . .
RUN dotnet publish "websimulador.csproj" -c Release -o /app/out

# Etapa final para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "websimulador.dll"]
