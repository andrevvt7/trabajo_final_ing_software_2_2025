# Trabajo Final Ing Software 2
##  Contenerizacion de una Aplicación Web MVC en .NET utilizando Docker
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
