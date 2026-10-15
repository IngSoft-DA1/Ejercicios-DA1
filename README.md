# Ejercicio DTO de Usuarios, con agente

Ejercicio guiado de 60 minutos para entender para qué sirve un DTO: desacoplar el frontend de las
entidades de dominio y ocultar información que no debería salir de la lógica de negocio, como la
contraseña de un usuario. Esta vez el código no lo escriben los alumnos: lo escribe un agente de IA
de consola (**OpenCode**) y los alumnos lo dirigen, revisan el diff y commitean a mano.

| | |
|---|---|
| Duración | 60 minutos |
| Modalidad | Grupos de 3 o parejas |
| Nivel | Básico |
| Herramientas | Rider, SDK de .NET 9 (con .NET 10 también funciona), git y OpenCode v2 con el modelo gratuito Big Pickle |
| Material | Solución inicial `LabDTO-agente.zip` (repositorio git en la rama `ejercicio-agente`, con `AGENTS.md` y `opencode.jsonc`) |

## De qué trata

Reciben una solución Blazor que ya funciona, separada en tres capas. La aplicación lista usuarios y
permite dar de alta nuevos, pero la página trabaja directo con la entidad de dominio: muestra las
contraseñas en texto plano y decide ella misma la contraseña de los usuarios nuevos.

El ejercicio consiste en detectar esos problemas y corregirlos con un DTO y un mapper, pero
trabajando con un agente: el grupo diagnostica, le pide un plan, lo corrige, le encarga los cambios
en ciclos cortos y revisa cada diff antes de commitear.

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
- **Antes de arrancar:** requisitos y los comandos para verificar .NET y git.
- **Uso de IA:** las reglas de trabajo con el agente y el ciclo que se repite en cada tarea.
- **Cinco pasos numerados del 0 al 4**, con los pedidos para el agente, las listas para revisar el
  plan y los diffs, y los checkpoints.
- **Tablas para completar** en el navegador (diagnóstico, revisión del plan, revisión de diffs y
  registro del grupo). Las respuestas quedan guardadas en el navegador y se pueden borrar desde el pie.
- **Recuadros** dentro de los pasos: pistas, avisos, checkpoints y preguntas «Para pensar».
- **Vuelta extra** de tarea.

## El ciclo de trabajo

```
PREDECIR  →  PEDIR  →  REVISAR EL DIFF EN RIDER  →  VERIFICAR (build/tests)  →  COMMIT A MANO
```

- **El agente propone, ustedes deciden.** Nada queda sin que alguien del grupo lo haya leído.
- **El commit es siempre a mano, desde Rider.** El agente tiene prohibido hacer `git commit`.

## Los pasos

| Paso | Qué hacen los alumnos |
|---|---|
| 0. Instalar y conectar OpenCode | Instalan OpenCode v2 (desinstalando la v1 si la tenían), eligen Big Pickle con `/models`, generan `AGENTS.md` con `/init` y verifican que tenga las reglas de git que le prohíben al agente commitear, publicar o borrar trabajo. |
| 1. Diagnóstico | Usan la app, leen la entidad, el servicio y la página, y responden tres preguntas **primero sin el agente** y después con él en modo Plan, comparando las respuestas. |
| 2. Plan antes de código | Repasan qué es un DTO y un mapper, le piden al agente en modo Plan una propuesta sin editar nada y la revisan con una lista antes de aprobarla. |
| 3. Ejecución en tres ciclos | **A:** DTO + mapper en BusinessLogic. **B:** el servicio entrega y recibe DTOs. **C:** la página trabaja con `UsuarioDto`. En cada ciclo predicen, piden, revisan el diff con una lista, verifican y commitean a mano. |
| 4. Cierre y verificación | Buscan menciones del dominio en Web, prueban la app en el navegador y le piden al agente que agregue a `AGENTS.md` tres reglas basadas en lo que tuvieron que corregir. Completan el registro del grupo. |

**Vuelta extra:** investigar otros agentes de consola y qué archivo de contexto lee cada uno
(`AGENTS.md`, `CLAUDE.md`, etc.).

## Qué se busca mostrar

- **El DTO desacopla las capas.** El frontend depende solo del DTO, no de cómo está armada la
  entidad de dominio. Si la entidad cambia, se ajusta la traducción en BusinessLogic y la página no
  se entera.
- **El DTO oculta información.** Un dato que no está en el DTO no llega al frontend: la página no
  puede mostrar la contraseña porque no existe en la clase que recibe.
- **El mapper concentra la traducción.** La conversión en los dos sentidos queda en una sola clase
  de BusinessLogic, la única capa que conoce la entidad y el DTO.
- **El agente no reemplaza el criterio.** Para juzgar su plan y sus diffs hay que entender los
  conceptos. Un pedido corto deja las decisiones en manos del agente; uno con objetivo,
  restricciones y criterio de aceptación las deja en manos del grupo.

## Conceptos que cubre

- Separación en capas: Domain, BusinessLogic y Web
- Referencias entre proyectos y su transitividad
- Entidad de dominio vs. DTO
- Mapper (`ToDto` y `ToEntity`)
- Servicios que entregan y reciben DTOs
- Ocultar datos sensibles al frontend
- Trabajo con un agente de consola: modos Plan y Build, `AGENTS.md`, pedidos precisos
- Revisión de diffs y commits manuales en Rider

## Cómo está pensado

- El ejercicio empieza por leer y analizar código existente, y el grupo responde antes que el agente.
- Antes de cada pedido el grupo anota una predicción, para comparar después con lo que hizo el agente.
- Los errores del agente no se arreglan a mano: se corrigen con un pedido más preciso.
- Las preguntas «Para pensar» no traen la respuesta: quedan como puente hacia la clase teórica.