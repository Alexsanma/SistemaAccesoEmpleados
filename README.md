# SGCAE · Sistema de Gestión y Control de Acceso de Empleados

API REST para administrar empleados, sus credenciales, los puntos de acceso y el registro de entradas y salidas de una empresa de logística. Construida con **ASP.NET Core 10**, **Entity Framework Core** y **SQL Server**, aplicando **Clean Architecture**, **DDD** y **CQRS** (MediatR).

## Arquitectura

```
src/
├── AccessControl.Domain          Entidades, objetos de valor, reglas de negocio y contratos de repositorio
├── AccessControl.Application     Casos de uso CQRS (Commands / Queries), validaciones, patrón Result
├── AccessControl.Infrastructure  EF Core, SQL Server, repositorios, Unit of Work y migraciones
└── AccessControl.Api             Controladores REST, respuestas estandarizadas y Swagger
tests/
└── AccessControl.Domain.Tests    Pruebas unitarias de las reglas de dominio
```

Dependencias: `Api → Application → Domain` y `Infrastructure → Application → Domain`. El dominio no depende de ninguna otra capa.

**Dominio**
- Agregados: `Employee`, `Credential`, `AccessPoint`, `AccessRecord`.
- Objetos de valor: `DocumentNumber`, `Email`, `CredentialCode`.
- `AccessPolicy` decide si un movimiento se autoriza o se rechaza.

**Reglas principales**
- La identificación y el correo del empleado son únicos.
- Un empleado inactivo no puede actualizarse ni recibir credenciales.
- Un empleado solo puede tener una credencial vigente o bloqueada; para asignarle otra, primero hay que revocar la actual. La revocación es definitiva.
- Se rechaza el acceso si la credencial no existe o está bloqueada o revocada, si el empleado está inactivo, si el punto de acceso está deshabilitado, si se registra una entrada sin haber salido o si se registra una salida sin entrada previa.
- Todos los intentos se guardan, tanto los autorizados como los rechazados (con su motivo).

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- Docker (para SQL Server) o una instancia propia de SQL Server

## Cómo ejecutar

1. Levantar SQL Server:

   ```bash
   docker compose up -d
   ```

2. Ejecutar la API (aplica las migraciones y crea los puntos de acceso iniciales automáticamente):

   ```bash
   dotnet run --project src/AccessControl.Api --launch-profile https
   ```

3. Abrir Swagger en <https://localhost:7000/swagger> (o en <http://localhost:5000/swagger> con el perfil `http`).

Para usar otra instancia de SQL Server, cambia `ConnectionStrings:AccessControlConnection` en `src/AccessControl.Api/appsettings.json`.

Para ejecutar las pruebas:

```bash
dotnet test
```

## Endpoints

| Requerimiento | Método | Ruta |
|---|---|---|
| RF-01 Empleados | `POST` · `GET` | `/api/employees` (paginado: `pageNumber`, `pageSize`, `search`, `department`, `status`) |
| | `GET` · `PUT` | `/api/employees/{id}` |
| | `PATCH` | `/api/employees/{id}/deactivate` |
| RF-02 Credenciales | `POST` · `GET` | `/api/employees/{id}/credentials` |
| | `GET` | `/api/credentials/{id}` · `/api/credentials/code/{code}` |
| | `PATCH` | `/api/credentials/{id}/block` · `/unblock` · `/revoke` |
| RF-03 Entradas y salidas | `POST` | `/api/access-records` |
| RF-04 Historial | `GET` | `/api/access-records` (`employeeId`, `from`, `to`, `accessPointId`, `result`, `pageNumber`, `pageSize`) |
| RF-06 Puntos de acceso | `POST` · `GET` | `/api/access-points` (`isEnabled`, `type`) |
| | `GET` | `/api/access-points/{id}` |
| | `PATCH` | `/api/access-points/{id}/enable` · `/disable` |

Ejemplo para registrar una entrada:

```json
POST /api/access-records
{
  "credentialCode": "CRD-8UGCC835",
  "accessPointId": "47dae8b3-3afd-479b-9b00-ac13f0d78fdb",
  "movementType": "Entry"
}
```

Todas las respuestas tienen la misma estructura:

```json
{ "isSuccess": true, "data": { }, "message": "Acceso autorizado y registrado.", "errors": null }
```

Códigos HTTP: `200`/`201` si la operación es exitosa, `400` para errores de validación o de negocio, `404` si el recurso no existe y `500` para errores inesperados.

## Evidencias

En [`docs/SGCAE.postman_collection.json`](docs/SGCAE.postman_collection.json) hay una colección de Postman con el flujo completo: crear un empleado, asignarle una credencial, registrar entradas y salidas autorizadas y rechazadas, bloquear, desactivar, revocar y consultar el historial. Las peticiones guardan los identificadores en variables, así que basta con ejecutarlas en orden.
