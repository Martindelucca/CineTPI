# TPI - Programación II (TUP - UTN FRC)
## Sistema de Gestión y Venta de Entradas de Cine

Este proyecto es el Trabajo Práctico Integrador (TPI) para la materia Programación II de la Tecnicatura Universitaria en Programación (TUP) de la UTN-FRC.

El objetivo es aplicar conceptos de POO avanzada, arquitectura en capas, patrones de diseño y desarrollo cliente-servidor (full-stack) con C# y .NET.

### 📋 Dominio Elegido
“Venta de entradas con butacas numeradas en un complejo de cines de la Ciudad de Córdoba.”

---

### 🛠️ Tecnologías Utilizadas

* **Backend (API):** C# 12, ASP.NET Core 8.0
* **Base de Datos:** Microsoft SQL Server
* **Acceso a Datos:** Entity Framework (EF) Core
* **Autenticación:** JWT (JSON Web Tokens)
* **Frontend (Web):** HTML5, CSS3 y JavaScript (ES6+)
* **Arquitectura:** Diseño en 3 Capas (Dominio, API, Web)

---

### 🏛️ Arquitectura de la Solución

La solución (`CineTPI.sln`) está dividida en 3 proyectos:

1.  **`CineTPI.Domain` (Librería de Clases):**
    * Contiene las **Entidades** (POCOs) que mapean la base de datos.
    * Define las **Interfaces** (Patrón Repositorio).
    * Implementa los **Repositorios** (lógica de acceso a datos con EF Core).
    * Define los **DTOs** (Data Transfer Objects) para la comunicación segura con la API.
    * Contiene los **Servicios** (lógica de negocio, ej: `AuthService`).

2.  **`CineTPI.API` (ASP.NET Core Web API):**
    * Expone los **Controladores** y *endpoints* RESTful (ej: `/api/peliculas`, `/api/reservas`).
    * Maneja la **Autenticación** y **Autorización** (validación de tokens JWT).
    * Sirve los archivos estáticos del frontend (`wwwroot`).

3.  **`wwwroot` (Frontend - Archivos Estáticos):**
    * Contiene las vistas (`.html`), estilos (`.css`) y lógica de cliente (`.js`).
    * Consume la `CineTPI.API` usando `fetch`.

---

### 🚀 Cómo Ejecutar el Proyecto

1.  **Base de Datos:**
    * Asegurarse de tener una instancia de SQL Server.
    * Modificar la `DefaultConnection` en el archivo `CineTPI.API/appsettings.Development.json` con sus credenciales.
    * Ejecutar el script de base de datos (`[NombreDeTuScript].sql`) para crear el esquema y los datos de prueba.

2.  **Backend:**
    * Abrir la solución `CineTPI.sln` con Visual Studio 2022.
    * Establecer `CineTPI.API` como proyecto de inicio.
    * Presionar F5 para iniciar el servidor.

3.  **Frontend:**
    * El servidor se iniciará automáticamente. Navegar a la URL:
    * `https://localhost:[TU_PUERTO]/login.html`

### 👤 Responsables (Team)

* **Martin de Lucca** - [405200
]