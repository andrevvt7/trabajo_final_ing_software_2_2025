# Trabajo Final de Ingeniería de Software 2
##  Contenerizacion de una Aplicación Web MVC en .NET utilizando Docker desde Github Codespaces
La contenerización es una técnica de despliegue de software que permite empaquetar una aplicación junto con todas sus dependencias en una unidad ejecutable aislada llamada contenedor. Actualmente, esta técnica constituye una práctica estándar en ingenieria de software para asegurar portabilidad, reproductibilidad y consistencia entre entornos.

En el desarrollo de aplicaciones web bajo el patrón MVC, la contenerizacion permite:
- Aislar el entorno de ejecucion.
- Eliminar dependecias del sistema operativo local.
- Facilitar despliegues repetibles.
- Mejorar la escalabilidad y mantenibilidad.

En este trabajo se describirá paso a paso como contenerizar una aplicación ASP.NET Core MVC con base de datos SQLite utilizando Docker en GitHub Codespaces, permitiendo ejecutar la aplicación sin depender del entorno local.

Tecnologias utilizadas:
- ASP.NET Core MVC (framework web de Microsoft).
- SQLite (Base de Datos).
- Docker (Motor de contenedores).
- Docker Compose (Orquestación).
- GitHub Codespaces (Entorno remoto de Desarrollo).

### Componentes principales
La contenerización con Docker se basa en un ecosistema de componentes interconectados, los principales son:
- Docker Engine: una tecnología cliente-servidor que permite ejecutar Docker, compuesta por un demonio (dockerd) para gestionar contenedores, imágenes y volúmenes.
- Dockerfile: un archivo de texto que contiene las instrucciones para automatizar la creación de una imagen Docker.
- Imágenes (Images): son plantillas inmutables de solo lectura que contienen el código, bibliotecas, dependencias y configuraciones necesarias para ejecutar una aplicación.
- Contenedores (Containers): instancias ligeras y ejecutables de una imagen.
- Registro (Registry/Repository): es un almacenamiento centralizado y compartido de imágenes. Docker Hub es el registro público principal.
- Volúmenes (Volumes): son mecanismos de almacenamiento persistente que permiten conservar datos generados por los contenedores, independientemente de su ciclo de vida.

<img width="708" height="433" alt="image" src="https://github.com/user-attachments/assets/13d04b1e-f8f8-49bf-89fb-d0c6911cb9e4" />

### Uso de Codespaces y beneficio respecto a Docker
En el contexto del desarrollo en la nube, GitHub Codespaces simplifica el uso de Docker, ya que el entorno de ejecución ya incluye Docker Engine preinstalado y configurado. Esto elimina la necesidad de instalar Docker localmente, permitiendo construir imágenes y ejecutar contenedores directamente en una máquina virtual remota.

## Desarrollo práctico
### Pasos previos
Lo primero que tenemos que tener listo es el repositorio de nuestro proyecto en Github y nuestro espacio en Codespaces. Para ello puede descargar el archivo .zip de este repositorio utilizando la opción Code → Download ZIP. Una vez descargado, descomprima el contenido y cree un nuevo repositorio propio, cargando allí los archivos obtenidos. Luego elimine los archivos "Dockerfile" y "docker-compose.yml" para empezar de cero con Docker.

### Dockerfile
Una vez que tenemos el repositorio y el codespace, **creamos un archivo con el nombre "Dockerfile"** en la carpeta principal del proyecto.
Un Dockerfile es un documento de texto que se utiliza para crear una imagen de contenedor. Proporciona instrucciones al generador de imágenes sobre los comandos a ejecutar, los archivos a copiar, el comando de inicio y más. Cada una de estas instrucciones genera una capa nueva en la imagen.
Más adelante se explica el uso de multi-stage builds en el Dockerfile.

##### Contenido del Dockerfile. Copie las siguientes instrucciones en el archivo:
```dockerfile
#Primera etapa: usamos una imagen oficial de .NET 8 con el SDK que incluye las herramientas de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS etapa-build
# Definimos un directorio de trabajo dentro del contenedor, donde se ejecutará lo que sigue
WORKDIR /app
# Copiamos todo el proyecto desde el repositorio al contenedor, desde la carpeta actual a /app
COPY . .
# Compilamos y preparamos la aplicación para producción. Los archivos compilados se generarán en la carpeta out
RUN dotnet publish -c Release -o out

#Segunda etapa: usamos una imagen que solo tiene el runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS etapa-final
# Se define un directorio de trabajo para esta imagen
WORKDIR /app
# Ahora copiamos los archcivos compilados de la etapa anterior desde la carpeta out a la actual /app
COPY --from=etapa-build /app/out .
# Indicamos que se usará el puerto 8080 (que luego se abrirá en docker-compose.yml)
EXPOSE 8080
# Por último definimos el comando que se ejecutará cuando se inicie el contenedor (sería como usar dotnet tl2-proyecto-2024-andrevvt7.dll)
ENTRYPOINT ["dotnet", "tl2-proyecto-2024-andrevvt7.dll"]
```

##### Las instrucciones que usamos son:
- ```FROM <imagen>``` - para especificar la imagen base para la compilación
- ```WORKDIR <ruta>``` - donde se especifica el directorio de trabajo o la ruta en la imagen donde se copiarán los archivos y se ejecutarán los comandos.
- ```COPY <ruta-local> <ruta-imagen>``` - para indicarle al constructor que copie archivos del host (máquina local o el entorno donde se realiza la construcción de la imagen) y los coloque en la imagen del contenedor.
- ```RUN <comando>``` - para indicarle al constructor que ejecute el comando especificado.
- ```EXPOSE <número-puerto>```- para indicar un puerto que la imagen desea exponer.
- ```ENTRYPOINT <comando>``` - para definir el comando que el contenedor siempre ejecutará por defecto.

##### Multi-stage builds
En una compilación tradicional, todas las instrucciones de compilación se ejecutan en secuencia y en un único contenedor de compilación. Todas esas capas terminan en la imagen final.
En nuestro caso utilizamos la compilación de múltiples etapas (multi-stage builds) que permite introducir varias etapas en el Dockerfile, cada una con un propósito específico. Al separar el entorno de compilación del entorno de ejecución final, se puede reducir el tamaño de la imagen final y la superficie de ataque.
Nuestro Dockerfile utiliza dos etapas:
1. **etapa-build**: una etapa de compilación que utiliza una imagen base que contiene las herramientas necesarias para compilar la aplicación. Incluye comandos para instalar herramientas de compilación, copiar código fuente y ejecutar comandos de compilación.
2. **etapa-final**: que utiliza una imagen base más pequeña para ejecutar la aplicación. Copia los artefactos compilados desde la etapa de compilación. Finalmente, se define la instrucción ```ENTRYPOINT``` para iniciar la aplicación.
Para cada etapa usamos una declaración ```FROM``` y la palabra clave ```AS``` para asignarle el nombre a cada una. Además, la declaración ```COPY``` en la segunda etapa es ```COPY --from``` la etapa anterior.

### docker-compose.yml

Una vez creado el Dockerfile, el siguiente paso es crear un archivo llamado docker-compose.yml en la carpeta principal del proyecto.

Docker Compose es una herramienta que permite definir y ejecutar aplicaciones multicontenedor. En este caso, lo utilizamos para configurar y ejecutar nuestra aplicación ASP.NET dentro de un contenedor de manera sencilla, centralizando toda la configuración en un solo archivo.

El archivo _docker-compose.yml_ define los servicios, puertos, variables de entorno y volúmenes que utilizará nuestra aplicación.

### Contenido del docker-compose.yml

Copie las siguientes instrucciones en el archivo:
``` yml
services:
  web-app:
    build: .
    image: mi-proyecto-mvc
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_URLS=http://+:8080
      - ASPNETCORE_ENVIRONMENT=Development
    volumes:
      - ./Kanban.db:/app/Kanban.db 
```
### Explicación de cada sección
- **<font size="3">Services</font>**: 
La sección services define los contenedores que formarán parte de la aplicación.
En este caso tenemos un único servicio llamado web-app, que representa nuestra aplicación ASP.NET.

- **<font size="3">web-app</font>**: 
Es el nombre del servicio. Puede ser cualquier nombre descriptivo.
Este nombre también funcionará como identificador interno dentro de la red de Docker.

- **<font size="3">build: . </font>**: 
Indica que Docker debe construir la imagen utilizando el Dockerfile que se encuentra en el directorio actual (.).

- **<font size="3">image: mi-proyecto-mvc </font>**: 
Define el nombre que tendrá la imagen generada.
Esto permite reutilizarla posteriormente sin necesidad de volver a construirla si no hubo cambios.

- **<font size="3">ports</font>**:
``` yml
 ports: "8080:8080"
```
 Realiza el mapeo de puertos:

``` yml
 PUERTO_PC : PUERTO_CONTENEDOR
```
En este caso:

- El puerto 8080 de la PC
- Se conecta con el puerto 8080 del contenedor

Esto permite acceder a la aplicación desde el navegador ingresando a:
```
http://localhost:8080
```
- **<font size="3">environment </font>**:
Permite definir variables de entorno dentro del contenedor.

environment:
``` yml
  - ASPNETCORE_URLS=http://+:8080
  - ASPNETCORE_ENVIRONMENT=Development
```
**<font size="2">ASPNETCORE_URLS </font>**:
Indica en qué puerto escuchará la aplicación dentro del contenedor.

` http://+:8080 ` significa:

- Escuchar en cualquier interfaz de red (`+`)
- En el puerto 8080

**<font size="2">ASPNETCORE_ENVIRONMENT </font>**:
Define el entorno de ejecución.

En este caso:

**`Development`**  → habilita modo desarrollo (mensajes de error detallados, herramientas de debugging, etc.)

- **<font size="3">volumes </font>**:
```yml
volumes:
  - ./Kanban.db:/app/Kanban.db
```

Los volúmenes permiten vincular archivos o carpetas de la máquina host (PC) con el contenedor.

En este caso:
```
./Kanban.db  → archivo en la PC
/app/Kanban.db → archivo dentro del contenedor
```
Esto significa que:
- La base de datos SQLite se guarda físicamente en la PC.
- Si el contenedor se elimina, la base de datos no se pierde.
- Los cambios realizados dentro del contenedor se reflejan en el archivo local.
Esto es fundamental para persistencia de datos.

### Comó Ejecutar la Aplicación
Existen al menos 3 opciones para ejecutar la aplicacion, desde la terminal de codespaces hacemos:

#### OPCIÓN 1 - Construir y Ejecutar
``` bash
docker compose up --build
```
**¿QUÉ HACE?**
1. Construye la imagen (si no existe o cambió el Dockerfile).
2. Crea los contenedores.
3. Inicia los servicios.
4. Muestra los logs en tiempo real.

#### OPCIÓN 2 - Modo desarrollo watch
``` bash
docker compose watch
```
**¿QUÉ HACE?**
1. Observa cambios en el código.
2. Reconstruye automaticamente cuando detecta modificaciones.
3. Ideal para desarrollo activo.

#### OPCIÓN 3 - Ejecutar en segundo plano
``` bash
docker compose up -d
```
Ejecuta los contenedores en modo detached (segundo plano).

**¿QUÉ SUCEDE INTERNAMENTE?**
1.  Docker lee el `Dockerfile`.
2. Construye una **imagen**.
3. A partir de la imagen crea un **contenedor**.
4. El contenedor se ejecuta dentro del servidor remoto del Codespace.
5. GitHub expone automaticamente los puertos utilizados.

- La imagen es estática.
- El contenedor es la instancia de ejecución.

### ¿Dónde se ejecuta realmente?
Todo se ejecuta en un servidor remoto proporcionado por Github. La aplicación no corre en la computadora local, sino en la insfractructura en al nube GitHub.

#### Luego de ejecutar la aplicación puede ingresar a la aplicación con el usuario "admin" y la contraseña "admin", para poder modificar los datos de la base de datos y probar el funcionamiento del uso de volúmenes.

#### ¿Qué pasaría si no usaramos el archivo de configuración docker-compose.yml?
Si no se usara ese archivo cada uno del equipo debería ejecutar los siguientes comandos:
``` bash
docker build -t <nombre_proyecto> .
docker run -p 8080:8080 -v <nombre_volumen>:/app/kanban.db <nombre_proyecto>
```
Lo anterior significa que deberían poner el nombre del proyecto manualmente, al igual que la dirección de la base de datos, lo genera pérdida de estandarización. Entonces, podemos decir que:
1. Sin compose:
   - Se escriben más comandos manuales.
   - Hay más posibilidad de errores.
   - Es menos estandarizado.
2. Con compose:
   - Se construye y ejecuta el proyecto con un solo comando.
   - Está todo definido en un archivo versionado.
   - Se simplifica el trabajo en equipo.
   
## Beneficios de implementar la técnica
La implementación de contenedores mediante Docker y el uso de entornos en la nube como GitHub Codespaces aporta múltiples beneficios tanto en el área específica del desarrollo como en otras áreas relacionadas de la ingeniería de software.
1. **Reproducibilidad del entorno**
Permite garantizar que todos los desarrolladores trabajen bajo las mismas condiciones técnicas (versiones de dependencias, sistema operativo, configuración). Esto reduce errores asociados a diferencias de entorno.
2. **Portabilidad**
Los contenedores pueden ejecutarse en cualquier infraestructura que soporte Docker, ya sea local, en servidores físicos o en la nube. Esto facilita la migración entre entornos (desarrollo, testing y producción).
3. **Integración con prácticas DevOps**
La contenerización favorece la automatización, la integración continua y el despliegue continuo (CI/CD), alineándose con principios del movimiento DevOps. Esto mejora la eficiencia del ciclo de vida del software.
4. **Aislamiento de aplicaciones**
Cada contenedor funciona de manera independiente, evitando conflictos entre proyectos que utilicen diferentes versiones de librerías o tecnologías.
5. **Escalabilidad**
En entornos productivos, los contenedores pueden replicarse fácilmente para soportar mayor carga de usuarios.
6. Reducción del tiempo de configuración
El uso de Codespaces elimina la necesidad de instalar herramientas localmente, reduciendo tiempos de onboarding en equipos nuevos.

## Desafíos y consideraciones
A pesar de sus ventajas, la implementación de estas tecnologías también presenta desafíos que deben ser considerados por los profesionales.
1. **Curva de aprendizaje**
Docker introduce conceptos nuevos como imágenes, contenedores, volúmenes y redes. Requiere capacitación inicial para su uso correcto.
2. **Gestión de recursos**
Los contenedores consumen CPU y memoria. En entornos limitados (como planes gratuitos de Codespaces), pueden presentarse restricciones de rendimiento.
3. **Seguridad**
Una mala configuración de imágenes, puertos o variables de entorno puede exponer información sensible.
4. **Persistencia de datos**
Si no se utilizan volúmenes correctamente, los datos pueden perderse al eliminar contenedores.
5. **Dependencia de servicios en la nube**
El uso de Codespaces implica depender de la disponibilidad de la infraestructura de GitHub.
6. **Complejidad en proyectos grandes**
En arquitecturas más complejas (microservicios), la configuración puede volverse extensa y requerir herramientas adicionales como orquestadores.

## Conclusión
La contenerización de una aplicación ASP.NET Core MVC mediante Docker, integrada con GitHub Codespaces, demuestra cómo es posible estandarizar el entorno de desarrollo y ejecución de forma práctica y profesional. A través del uso de Dockerfile y Docker Compose, se logra estructurar el proceso de construcción, configuración y despliegue de la aplicación de manera clara, automatizada y reproducible.
Esta práctica no solo mejora la organización técnica del proyecto, sino que también fortalece el trabajo colaborativo, reduce inconsistencias entre entornos y prepara la aplicación para escenarios reales de despliegue.

## Referencias
[¿Qué es Docker?](https://docs.docker.com/get-started/docker-overview/)  
[La arquitectura Docker](https://docs.docker.com/get-started/docker-overview/#docker-architecture)  
[¿Qué es una imagen?](https://docs.docker.com/get-started/docker-concepts/the-basics/what-is-an-image/)  
[¿Qué es un contenedor?](https://docs.docker.com/get-started/docker-concepts/the-basics/what-is-a-container/)  
[¿Qué es Dockerfile?](https://docs.docker.com/get-started/docker-concepts/building-images/writing-a-dockerfile/)  
[Construcción en múltiples etapas](https://docs.docker.com/get-started/docker-concepts/building-images/multi-stage-builds/)  
[¿Qué es Docker compose?](https://docs.docker.com/get-started/docker-concepts/the-basics/what-is-docker-compose/)  
[Persistencia de datos](https://docs.docker.com/get-started/docker-concepts/running-containers/persisting-container-data/)  
[Multi contenedores](https://docs.docker.com/get-started/docker-concepts/running-containers/multi-container-applications/)  
[Codespaces y Docker](https://medium.com/@michelle.xie/explain-by-example-github-codespaces-dev-containers-53684f8fe6d7#:~:text=GitHub%20Codespaces%20reimagined%20development%20from,file%20is%20used%20to%20do.)
