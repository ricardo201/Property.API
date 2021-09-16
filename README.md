# Property.API

Proyecto para administrar propiedades.

### Pre-requisitos 📋

- Visual Studio 2019
- Microsoft SQL

### Instalación 🔧

Una vez descargado el proyecto se deben seguir los siguientes pasos para ejecutarlo en un ambiente local:
- Abrir el proyecto con Visual Studio 2019
- Modificar la cadena de conexión a la base de datos que se encuentra en el archivo appsettings.json del proyecto PropertyBuilding.API
- Abrir la consola para desarrolladores de PowerShell y correr las migraciones con el siguiente comando desde el directorio del proyecto PropertyBuilding.API
``` 
 dotnet ef database update
```
Ejecutar el proyecto PropertyBuilding.API

## Pruebas Unitarias ⚙️

Las pruebas unitarias fueron realizadas con NUnit y se encuentran en el proyecto PropertyBuilding.Test

## Notas

- El proyecto se puede mejorar utilizando servicios de almacenamiento en la nube como S3 de AWS para permitir que los servidores desde donde se despliegue la aplicación puedan ser Stateless y permita su funcionamiento en una arquitectura de alta disponibilidad.
- Para el manejo de las credenciales y accessos debase de datos se pueden implementar servicios en la nube de almacenamiento de credenciales.
- Se puede optimizar el acceso a los datos de login haciendo uso de sistemas de base de datos de memoria como Redis

## Autor ✒️

* **Ricardo Gonzalez Cubillos**   
