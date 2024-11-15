# Usa la imagen de .NET SDK para compilar el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia los archivos .csproj y restaura las dependencias
COPY *.csproj . 
RUN dotnet restore

# Copia el resto de los archivos del proyecto y construye la aplicación
COPY . . 
RUN dotnet publish -c Release -o /out

# Usa una imagen de .NET Runtime para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Copia los archivos compilados desde la etapa de construcción
COPY --from=build /out . 

# Expone el puerto 80 para el contenedor
EXPOSE 8090

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "UserProfileService.dll"]
