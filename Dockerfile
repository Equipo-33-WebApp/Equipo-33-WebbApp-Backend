# Etapa 1: Compilación
# Usamos la imagen del SDK de .NET 8.0 para compilar la aplicación.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos .csproj y restaurar las dependencias primero.
# Esto aprovecha el cache de Docker. Si los archivos de proyecto no cambian,
# no se volverán a descargar las dependencias.
COPY ["Fintech.WebAPI/Fintech.WebAPI.csproj", "Fintech.WebAPI/"]
COPY ["Fintech.Application/Fintech.Application.csproj", "Fintech.Application/"]
COPY ["Fintech.Domain/Fintech.Domain.csproj", "Fintech.Domain/"]
COPY ["Fintech.Infrastructure/Fintech.Infrastructure.csproj", "Fintech.Infrastructure/"]
RUN dotnet restore "Fintech.WebAPI/Fintech.WebAPI.csproj"

# Copiar el resto del código fuente
COPY . .
WORKDIR "/src/Fintech.WebAPI"

# Compilar la aplicación en modo Release
RUN dotnet build "Fintech.WebAPI.csproj" -c Release -o /app/build

# Etapa 2: Publicación
# Generar los artefactos de publicación a partir de la compilación anterior.
FROM build AS publish
RUN dotnet publish "Fintech.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 3: Final
# Usamos la imagen de ASP.NET Core runtime, que es más ligera que la del SDK.
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Exponer el puerto 8080. ASP.NET Core en contenedores escucha en este puerto por defecto.
EXPOSE 8080

# Copiar los artefactos publicados de la etapa 'publish'
COPY --from=publish /app/publish .

# Definir el punto de entrada para ejecutar la aplicación
ENTRYPOINT ["dotnet", "Fintech.WebAPI.dll"]

# Opcional: Crear un usuario no-root para mejorar la seguridad
RUN useradd -m appuser
USER appuser