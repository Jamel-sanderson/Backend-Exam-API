# 🧩 API REST - Gestión de Cursos y Estudiantes

Este es el backend del sistema de gestión de cursos y estudiantes. Proporciona una API RESTful desarrollada en ASP.NET Core que permite realizar operaciones CRUD sobre cursos y estudiantes, y envía notificaciones push mediante Firebase Cloud Messaging (FCM).

---

## 🧰 Tecnologías utilizadas

- ASP.NET Core
- Entity Framework Core
- SQL Server
- Firebase Admin SDK (para FCM)

---

## ⚙️ Requisitos

- .NET 7 SDK o superior
- SQL Server 
- Herramienta como Postman (opcional, para pruebas)

---

## 🚀 Configuración del proyecto

1. Clonar el repositorio:

   ```bash
   git clone https://github.com/Jamel-sanderson/Backend-Exam-API
   cd StudentCoursesSystem
   ```

2. Configurar tu archivo `appsettings.json`:

   - Define la cadena de conexión a tu base de datos SQL Server.

3. Crear y ejecutar las migraciones para crear la base de datos:

   ```bash
   dotnet ef migrations add mig 
   dotnet ef database update
   ```

4. Iniciar la API:

   ```bash
   dotnet watch run
   ```

   La API se ejecutará en `https://localhost:5000`.
   Se puede acceder a `https://localhost:5000/swagger` para probar la API.

---

## 🔔 Notificaciones Push

Al registrar un nuevo estudiante, la API enviará una notificación push con el mensaje:

```
Estudiante: [nombre del estudiante], se ha inscrito al curso: [nombre del curso]
```

---

## 🌐 Endpoints Principales

```
GET    /api/courses
GET    /api/courses/{id}
POST   /api/courses
PUT    /api/courses/{id}
DELETE /api/courses/{id}

GET    /api/student
GET    /api/student/{id}
POST   /api/student
PUT    /api/student/{id}
DELETE /api/student/{id}
```

---

## 🧑‍💻 Autores

- [Jamyr González García] – [Github](https://github.com/jamyr17)
- [Jamel Sandí] – [Github](https://github.com/Jamel-sanderson)
