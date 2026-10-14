# LabEF (solución inicial)

Solución de partida para el ejercicio de Entity Framework Core (code first).

```
LabEF.sln
docker-compose.yml          SQL Server en Docker
├── LabEF.Domain/           Vehiculo, Auto y Moto
├── LabEF.DataAccess/       SqlContext y VehiculoRepositorio   → referencia a Domain
└── LabEF.Web/              Blazor (página Vehículos)          → referencia a DataAccess
```

1. Levantá la base desde la carpeta de la solución: `docker compose up -d`
2. Abrí `LabEF.sln` con Rider y corré `LabEF.Web` (perfil `http`): http://localhost:5090

Requiere el SDK de .NET 9, Docker y la herramienta `dotnet-ef`.
