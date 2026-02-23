# La primera etapa nos servirá para realizar solo la compilación
# Usamos una imagen oficial de .NET 8 con el SDK que incluye las herramientas de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS etapa-build
# Definimos un directorio de trabajo dentro del contenedor, donde se ejecutará lo que sigue
WORKDIR /app
# Copiamos todo el proyecto desde el repositorio al contenedor, desde la carpeta actual a /app
COPY . .
# Compilamos y preparamos la aplicación para producción. Los archivos compilados se generarán en la carpeta out
RUN dotnet publish -c Release -o out

# En la segunda etapa configuramos la ejecución
# Usamos una imagen que solo tiene el runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS etapa-final
# Se define un directorio de trabajo para esta imagen
WORKDIR /app
# Ahora copiamos los archcivos compilados de la etapa anterior desde la carpeta out a la actual /app
COPY --from=etapa-build /app/out .
# Indicamos que se usará el puerto 8080 (que luego se abrirá en docker-compose.yml)
EXPOSE 8080
# Por último definimos el comando que se ejecutará cuando se inicie el contenedor (sería como usar dotnet tl2-proyecto-2024-andrevvt7.dll)
ENTRYPOINT ["dotnet", "tl2-proyecto-2024-andrevvt7.dll"]