# Ejercicio DTO de Usuarios

Ejercicio para entender para qué sirve un DTO: desacoplar el frontend de las
entidades de dominio y ocultar información que no debería salir de la lógica de negocio, como la
contraseña de un usuario.

| | |
|---|---|
| Herramientas | Rider y SDK de .NET 9 (con .NET 10 también funciona) |
| Material | Solución inicial `LabDTO-inicial.zip` |

## De qué trata

Reciben una solución Blazor que ya funciona, separada en tres
capas. La aplicación lista usuarios y permite dar de alta nuevos, pero la página trabaja directo
con la entidad de dominio: muestra las contraseñas en texto plano y decide ella misma la
contraseña de los usuarios nuevos.

El ejercicio consiste en leer el código, detectar esos problemas y corregirlos creando un DTO y un
mapper, de modo que el frontend deje de conocer la entidad de dominio.

## La solución inicial

```
LabDTO.sln
├── LabDTO.Domain/          entidad Usuario (con Password)
├── LabDTO.BusinessLogic/   UsuarioServicio                 → referencia a Domain
└── LabDTO.Web/             frontend Blazor (Usuarios.razor) → referencia a BusinessLogic
```

La app corre en `http://localhost:5080`.

## Qué tiene la página

- Encabezado con el título, una descripción y los conceptos que se trabajan.
- **Antes de arrancar:** requisitos y el comando para verificar la versión de .NET.
- **Seis pasos numerados del 0 al 5**, con el código de partida y los `TODO` (del 1 al 6) que el
  alumno completa.
- **Tabla de análisis** en el paso 1, con preguntas para responder en grupo antes de tocar el código.
- **Recuadros** dentro de los pasos: pistas, checkpoints y preguntas «Para pensar».
- **Vuelta extra** opcional.

## Los pasos

| Paso | Qué hacen los alumnos |
|---|---|
| 0. Abrir la solución | Abren `LabDTO.sln` en Rider y corren `LabDTO.Web`. |
| 1. Recorrer la aplicación y el código | Usan la página, leen la entidad, el servicio y la página, y responden cinco preguntas: qué datos se muestran, de qué capa viene la clase que usa la página, cómo Web llega a usar Domain, quién decide la contraseña y qué se rompe si cambia la entidad. |
| 2. Crear el DTO | Crean `UsuarioDto` en BusinessLogic, eligiendo qué propiedades necesita el frontend y dejando afuera la contraseña. |
| 3. Crear el mapper | Crean `UsuarioMapper` con `ToDto` y `ToEntity`, y mueven ahí la contraseña por defecto que antes decidía la página. |
| 4. El servicio habla en DTOs | Cambian `UsuarioServicio` para que entregue y reciba `UsuarioDto`. BusinessLogic compila, pero Web no: los errores muestran de qué dependía el frontend. |
| 5. Arreglar la página | Hacen que la página trabaje con `UsuarioDto`, sin columna de contraseña y sin `@using LabDTO.Domain`. |

**Vuelta extra:** crear un DTO específico para el alta, sin `Id`, y una página *Directorio* con su
propio DTO que muestre solo los nombres.

## Qué se busca mostrar

- **El DTO desacopla las capas.** El frontend depende solo del DTO, no de cómo está armada la
  entidad de dominio. Si la entidad cambia, se ajusta la traducción en BusinessLogic y la página no
  se entera.
- **El DTO oculta información.** Un dato que no está en el DTO no llega al frontend, ni por error:
  la página no puede mostrar la contraseña porque no existe en la clase que recibe.
- **El mapper concentra la traducción.** El DTO no tiene lógica y la página no conoce la entidad,
  así que la conversión en los dos sentidos queda en una sola clase de BusinessLogic.

## Conceptos que cubre

- Separación en capas: Domain, BusinessLogic y Web
- Referencias entre proyectos y su transitividad
- Entidad de dominio vs. DTO
- Mapper escrito a mano (`ToDto` y `ToEntity`)
- Servicios que entregan y reciben DTOs
- Ocultar datos sensibles al frontend
