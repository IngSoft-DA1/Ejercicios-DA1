# Ejercicio EF Code First

Ejercicio guiado de 45 minutos para entender cómo trabaja Entity Framework Core con el enfoque
code first: el DbContext, las migraciones, las convenciones y cómo se guarda una jerarquía de
clases en una base relacional.

| | |
|---|---|
| Duración | 45 minutos |
| Modalidad | Grupos de 3 o parejas |
| Nivel | Básico |
| Herramientas | Rider, SDK de .NET 9, Docker y DBeaver |
| Material | Solución inicial en la carpeta `LabEF/` |

## Contenido

```
index.html, css/, js/     la letra del ejercicio (abrí index.html en el navegador)
LabEF/                    la solución inicial para abrir con Rider
├── LabEF.sln
├── docker-compose.yml    SQL Server en Docker
├── LabEF.Domain/         Vehiculo (abstracta), Auto y Moto
├── LabEF.DataAccess/     SqlContext y VehiculoRepositorio   → referencia a Domain
└── LabEF.Web/            Blazor (página Vehículos)          → referencia a DataAccess
```

## De qué trata

Los alumnos reciben las clases de dominio y una página Blazor sencilla, sin base de datos.
Escriben el `DbContext` y su registro, generan migraciones desde Rider y miran en DBeaver qué
tablas arma EF a partir de sus clases.

## Los pasos

| Paso | Qué hacen los alumnos |
|---|---|
| 0. Levantar la base | `docker compose up -d` y se conectan con DBeaver al SQL Server. |
| 1. Recorrer la solución | Leen las clases y qué es code first. |
| 2. El DbContext y su registro | Declaran los `DbSet` y registran el contexto. La app falla porque la base no existe. |
| 3. La primera migración | Generan la migración `Inicial` y la leen antes de aplicarla. |
| 4. Aplicar y mirar la base | Aplican la migración y ven una sola tabla con la columna `Discriminator` (herencia TPH). |
| 5. Cambiar el modelo | Agregan `Color` sin migración, ven el error y lo resuelven con una migración nueva. |

**Vuelta extra:** cambiar una convención con `HasMaxLength` y generar una migración con la
estrategia TPT para comparar.

## Conceptos que cubre

- Code first y DbContext
- Convenciones de EF (nombres de tablas, clave primaria, tipos y nulabilidad)
- Migraciones: crear, leer y aplicar
- Herencia: TPH, TPT y TPC
- Desincronización entre el modelo y la base
