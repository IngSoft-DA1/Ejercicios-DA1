# Ejercicio DTO de Usuarios, con agente

*Blazor · Ejercicio guiado con agente de IA · 60 min*

Te damos una aplicación Blazor separada en tres capas (Domain, BusinessLogic y Web) que ya funciona: lista usuarios y permite dar de alta nuevos. Tu trabajo es el mismo del ejercicio original (encontrar qué está mal en cómo el frontend usa los datos y corregirlo con un DTO y un mapper), pero esta vez **no escribís el código vos**: lo dirigís, lo revisás y lo commiteás. El que escribe el código es un agente de IA que corre en la terminal: **OpenCode**.

`agente de consola` · `plan antes de editar` · `revisar el diff` · `commit manual` · `capas` · `entidad de dominio` · `DTO` · `mapper` · `ocultar la contraseña`

---

## Antes de arrancar

- **Rider** instalado y un navegador.
- El **SDK de .NET 9** (con .NET 10 también funciona).
- La solución inicial `LabDTO-agente.zip` que te pasa el docente. Ya es un repositorio **git**, está parada en la rama `ejercicio-agente` e incluye dos archivos que vas a leer en el Paso 0: `AGENTS.md` y `opencode.jsonc`.
- **OpenCode v2** (lo instalás en el Paso 0) y una **clave de proveedor de modelo** que te da el docente. No la compartas ni la subas al repositorio.
- Tiempo: **60 min**, en grupos de 3 o en parejas.

```sh
# Terminal
dotnet --version   # tiene que empezar con 9
git --version      # cualquier versión
```

### Cómo se reparte el tiempo

| Bloque | Min | Qué hacen |
|---|---|---|
| **Paso 0. Arranque** | 5 | Instalar y conectar OpenCode, primer contacto en modo lectura |
| **Paso 1. Diagnóstico** | 8 | Responder primero ustedes, después preguntarle al agente |
| **Paso 2. Plan** | 7 | Pedir un plan, revisarlo y corregirlo antes de dejar editar |
| **Paso 3. Ejecución** | 30 | Tres ciclos: DTO + mapper (12), servicio (8), página (10) |
| **Paso 4. Cierre** | 10 | Chequeo de arquitectura, prueba en el navegador, reglas nuevas |

### Las reglas del juego

1. **El agente propone, ustedes deciden.** Nada queda sin que alguien del grupo lo haya leído.
2. **El commit es siempre a mano, desde Rider.** El agente tiene prohibido hacer `git commit` (lo vas a ver en su configuración).
3. **Cada tarea sigue el mismo ciclo:**

```
PREDECIR  →  PEDIR  →  REVISAR EL DIFF EN RIDER  →  VERIFICAR (build/tests)  →  COMMIT A MANO
```

4. **Roles que rotan en cada ciclo** (en parejas, repartan entre dos):
   - **Piloto:** escribe los pedidos al agente.
   - **Revisor:** lee el diff en Rider archivo por archivo y dice qué le parece mal.
   - **Escribano:** anota las predicciones y los hallazgos, y hace el commit.

---

## Paso 0. Instalar y conectar OpenCode

*terminal de Rider · 5 min*

> **💡 Si tu docente ya lo dejó instalado**, saltá directo a «Verificá la instalación».

Abrí la terminal integrada de Rider (**View → Tool Windows → Terminal**, o `Alt+F12`) y probá **una** de estas opciones.

### Opción A. Script de instalación (macOS, Linux y WSL)

```sh
curl -fsSL https://opencode.ai/v2/install | bash
```

### Opción B. Descargar el binario (cualquier sistema, la única que sirve en Windows nativo)

Descargá el archivo de tu plataforma, descomprimilo y poné la carpeta donde quedó el ejecutable `opencode` en tu `PATH`.

| Sistema | Descarga |
|---|---|
| Windows (x64) | https://opencode.ai/update/api/latest/cli/opencode/opencode-windows-x64.zip |
| Windows (ARM64) | https://opencode.ai/update/api/latest/cli/opencode/opencode-windows-arm64.zip |
| macOS (Apple silicon) | https://opencode.ai/update/api/latest/cli/opencode/opencode-darwin-arm64.zip |
| macOS (Intel) | https://opencode.ai/update/api/latest/cli/opencode/opencode-darwin-x64.zip |
| Linux (x64) | https://opencode.ai/update/api/latest/cli/opencode/opencode-linux-x64.tar.gz |
| Linux (ARM64) | https://opencode.ai/update/api/latest/cli/opencode/opencode-linux-arm64.tar.gz |

Si el binario normal no arranca en una máquina vieja, en la guía oficial hay una variante `baseline` de cada uno.

### Opción C. Gestores de paquetes (macOS y Linux)

La página oficial tiene una pestaña por gestor (Homebrew, npm, Bun, pnpm, Yarn, Vite+, AUR). Entrá a https://opencode.ai/v2/docs/, elegí tu gestor y copiá **el comando de esa pestaña**, así te asegurás de instalar la versión 2.

> **⚠️ Windows**
> La documentación de la versión 2 indica que los gestores de paquetes de Windows (Scoop, Chocolatey y similares) **no están soportados**. En Windows usá la Opción B, o WSL con la Opción A. Además, la terminal de Rider en Windows suele ser PowerShell, que no entiende `curl ... | bash`.

> **⚠️ ¿Ya tenías OpenCode instalado?**
> Las versiones 1 y 2 usan el mismo comando `opencode` y no conviven. Si instalaste la 1 con un gestor de paquetes, desinstalala antes de instalar la 2.

### Verificá la instalación

Cerrá y volvé a abrir la terminal (para que se actualice el `PATH`) y corré:

```sh
opencode --version
```

> **✅ Checkpoint**
> Imprime un número de versión que **empieza con 2**. Si dice `command not found`, probá otra opción. Si empieza con 1, tenés la versión vieja: mirá el recuadro de arriba.

### Conectar el modelo

Posicionate en la carpeta de la solución y arrancá el agente:

```sh
cd ruta/a/LabDTO
opencode
```

Dentro de OpenCode escribí `/connect`, elegí el proveedor que indique el docente y pegá la clave. Después podés revisar qué modelo quedó activo con `/models`.

> **💡 Pista: teclas útiles en OpenCode**
> - **Enter** envía el pedido. **Ctrl+J** (o Shift+Enter, si tu terminal lo soporta) agrega una línea nueva.
> - **Shift+Tab** cambia entre agentes, y `/agents` los lista. Los dos que importan hoy son **Plan** (analiza, no edita archivos) y **Build** (puede editar).
> - **`@`** adjunta un archivo al pedido: `Explicame @LabDTO.Web/Components/Pages/Usuarios.razor`.
> - **`/undo`** deshace el último pedido y los cambios de archivos que hizo; **`/redo`** los restaura. Sirve de red de seguridad, pero **no reemplaza al commit**.
> - Si la interfaz completa se ve mal en la terminal de Rider, probá `opencode mini`, una versión minimalista.

### Primer contacto: solo lectura

Cambiá al agente **Plan** (Shift+Tab hasta que el indicador lo muestre) y pedile:

```
Explicame las tres capas de esta solución y qué proyecto referencia a cuál.
```

Antes de seguir, abrí en Rider **`AGENTS.md`** y **`opencode.jsonc`** (están en la raíz de la solución) y leelos entre todos:

- `AGENTS.md` es lo que el agente lee al arrancar: la arquitectura y los comandos del proyecto.
- `opencode.jsonc` define los permisos del agente. Buscá qué comandos de git tiene prohibidos.

> **✅ Checkpoint**
> El agente respondió sobre las capas y el grupo sabe contestar: ¿qué dice `AGENTS.md`? ¿Qué le prohíbe `opencode.jsonc` al agente?

> **🤔 Para pensar**
> Compará la respuesta del agente con el diagrama del Paso 1. ¿Acertó en todo? ¿Por qué conviene arrancar un trabajo nuevo en modo Plan?

---

## Paso 1. Diagnóstico: primero ustedes, después el agente

*antes de tocar nada, entender qué hay · 8 min*

Entrá a **Usuarios** en la app (con **▶ Run** sobre `LabDTO.Web` en Rider, queda en `http://localhost:5080`), mirá la tabla y agregá un usuario con el formulario.

La solución está separada en capas. Cada flecha es una referencia entre proyectos: el de la izquierda puede usar las clases del de la derecha.

```
LabDTO.Web  ──▶  LabDTO.BusinessLogic  ──▶  LabDTO.Domain
(frontend)       (lógica de negocio)         (entidades)

LabDTO.Domain/Usuario.cs                       la entidad de dominio
LabDTO.BusinessLogic/UsuarioServicio.cs        lista y agrega usuarios
LabDTO.Web/Components/Pages/Usuarios.razor     la página que acabás de usar
```

Abrí esos tres archivos en Rider y leelos. Después hagan esto **en este orden**:

**1.** Respondan las tres preguntas **sin usar el agente** y anótenlo en la columna del grupo.
**2.** Recién ahora, pídanle al agente (en modo Plan) que las responda: `Respondé estas tres preguntas sobre @LabDTO.Web/Components/Pages/Usuarios.razor, @LabDTO.BusinessLogic/UsuarioServicio.cs y @LabDTO.Domain/Usuario.cs: ...`
**3.** Completen las otras dos columnas.

| Pregunta | Respuesta del grupo | Respuesta del agente | ¿Coinciden? ¿Quién tenía razón? |
|---|---|---|---|
| ¿Qué datos de cada usuario muestra la página? ¿Alguno no debería verse? | | | |
| El proyecto Web solo referencia a BusinessLogic. ¿Cómo puede entonces usar una clase de Domain? | | | |
| Cuando agregás un usuario, ¿quién decide su contraseña? ¿En qué capa está esa decisión? | | | |

> **✅ Checkpoint**
> Las tres filas están completas y el grupo puede explicar por qué, si en `Usuario` renombran `Email` a `Correo`, también deja de compilar el frontend.

> **🤔 Para pensar**
> La solución está separada en capas, pero la página trabaja directo con la entidad de dominio. ¿Para qué sirve separar en proyectos si el frontend termina conociendo todo igual? Si el agente coincidió con ustedes en todo, ¿seguirían revisando su trabajo?

---

## Paso 2. Plan antes de código

*cambiar al agente Plan, revisar, corregir, y recién ahí aprobar · 7 min*

Antes de pedir nada, dos conceptos. Los necesitan para juzgar si el plan del agente tiene sentido.

> **💡 Qué es un DTO**
> Un DTO (Data Transfer Object) es una clase sin lógica: solo propiedades, pensada para llevar datos de una capa a otra. No hereda de la entidad ni la referencia: es una clase aparte, con las propiedades que elijas.
>
> En este ejercicio es el «formato de intercambio» entre BusinessLogic y Web: el servicio entrega y recibe DTOs, y la entidad `Usuario` se queda adentro de BusinessLogic. Eso sirve para dos cosas:
>
> **Desacoplar capas.** El frontend depende solo del DTO. Si la entidad cambia (se renombra una propiedad, se agrega un campo interno), alcanza con ajustar la traducción en BusinessLogic: la página no se entera.
>
> **Ocultar información.** Un dato que no está en el DTO no llega al frontend, ni por error: no hay forma de mostrarlo porque no existe en la clase que recibe la página.

> **💡 Qué es un mapper**
> Alguien tiene que pasar los datos entre la entidad `Usuario` y el `UsuarioDto`, en los dos sentidos. Esa responsabilidad no es del DTO (tendría que conocer la entidad, justo lo que queremos evitar) ni de la página (que no debería saber que la entidad existe). Por eso va en una clase aparte, dedicada solo a eso: el **mapper**. Así toda la traducción queda en un único lugar, en la única capa que conoce las dos clases: BusinessLogic.

Con el agente en **Plan**, pidan:

```
Quiero que el frontend deje de usar la entidad Usuario. Proponé un plan con
DTO y mapper, sin editar nada. Decime qué archivos vas a crear o tocar.
```

Revisen el plan con esta lista antes de aprobarlo:

| Pregunta sobre el plan | ¿Cierra? |
|---|---|
| ¿Qué propiedades pone en el DTO? ¿Está la contraseña? | |
| ¿En qué proyecto y carpeta pone el DTO y el mapper? ¿Es la capa correcta? | |
| ¿El mapper se encarga de las dos direcciones (entidad → DTO y DTO → entidad)? | |
| ¿Dice quién va a decidir la contraseña de los usuarios nuevos? | |
| ¿Qué archivos toca? ¿Alguno que no debería? | |

Si algo no cierra, díganselo al agente y que ajuste el plan. **Cuando el plan esté bien**, cambien a **Build** con Shift+Tab. A partir de acá el agente puede editar archivos.

> **✅ Checkpoint**
> Tienen un plan que el grupo entiende y aprobó, y anotaron al menos una cosa que corrigieron o que el agente hizo bien sin que se la pidieran.

---

## Paso 3. Ejecución en tres ciclos

*cada ciclo: predecir → pedir → revisar el diff → verificar → commit a mano · 30 min*

### Cómo revisar el diff y commitear en Rider

- **Ver los cambios:** abrí **Git → Commit** (normalmente `Ctrl+K`, o `Cmd+K` en Mac). A la izquierda aparecen los archivos modificados; hacé doble clic en cada uno para ver el diff (lo viejo contra lo nuevo).
- **Commitear:** tildá los archivos que quieras incluir, escribí **un mensaje propio** que diga qué cambió y por qué, y apretá **Commit**.
- **Chequeo previo:** en la barra de estado de Rider tiene que decir `ejercicio-agente`, no `main`.

> **💡 Escribir buenos pedidos**
> Un pedido útil tiene tres partes:
> - **Objetivo:** qué quiero lograr.
> - **Restricciones:** qué no se puede hacer o tocar.
> - **Criterio de aceptación:** cómo sé que terminó bien (un comando que corre, algo que no debe aparecer).
>
> Un pedido corto como «hacé un DTO» deja todas las decisiones en manos del agente. A veces las acierta, pero no hay forma de saberlo sin leer el diff.

---

### Ciclo A. DTO + mapper (12 min)

**Predecir.** Antes de pedir, el escribano anota: ¿qué propiedades creen que va a poner el agente en el DTO? ¿En qué carpeta va a poner el mapper?

**Pedido 1, corto a propósito.** Experimenten:

```
Hacé un DTO de usuario y un mapper.
```

**Revisar el diff.** El revisor lee los archivos nuevos en Rider con esta lista:

| ¿Qué miro? | Cómo tiene que estar | ¿Está bien? |
|---|---|---|
| Propiedades del `UsuarioDto` | Solo lo que necesitan la tabla y el formulario. **Sin contraseña.** | |
| Referencias del DTO | No hereda de `Usuario` ni usa nada de `LabDTO.Domain` | |
| Lógica en el DTO | Ninguna: solo propiedades | |
| Ubicación del mapper | `LabDTO.BusinessLogic/Mappers` | |
| Sentidos del mapper | `ToDto(Usuario)` y `ToEntity(UsuarioDto)` | |
| Contraseña de los usuarios nuevos | La decide `ToEntity`, usando la misma que hoy asigna la página | |
| Archivos tocados | Solo en BusinessLogic: la página todavía no se toca | |

**Si hay algo mal**, no lo arreglen a mano: pídanselo con un pedido preciso (objetivo, restricciones, criterio de aceptación). Por ejemplo:

```
Corregí UsuarioDto y UsuarioMapper.
Objetivo: el DTO lleva solo Id, Nombre y Email; el mapper convierte en los dos sentidos.
Restricciones: el DTO no tiene contraseña, no referencia LabDTO.Domain y no tiene
lógica; el mapper va en LabDTO.BusinessLogic/Mappers; no toques LabDTO.Web.
La contraseña que hoy asigna la página a los usuarios nuevos pasa a decidirse en ToEntity.
Criterio de aceptación: "dotnet build LabDTO.BusinessLogic" compila sin errores.
```

> **💡 Pista**
> Si quieren comparar de verdad un pedido contra el otro, usen `/undo` para volver al estado anterior y prueben el pedido preciso desde cero. Miren qué cambia en el diff.

**Verificar y commitear.**

```sh
# Terminal
dotnet build LabDTO.BusinessLogic   # tiene que terminar en "Build succeeded."
```

Commit a mano desde Rider.

> **✅ Checkpoint**
> Compila. El DTO no tiene contraseña, el mapper vive en BusinessLogic y hay un commit hecho por ustedes. El escribano anotó **qué decidió mal el agente con el pedido corto** (o que no se equivocó, y cómo lo comprobaron).

---

### Ciclo B. El servicio habla en DTOs (8 min)

**Predecir.** Escriban antes de correr nada: *Si el servicio pasa a devolver y recibir DTOs, ¿qué proyecto va a dejar de compilar y en qué lugares?*

**Pedido:**

```
Cambiá UsuarioServicio para que hacia afuera hable solo en DTOs.
Objetivo: Listar() devuelve una lista de UsuarioDto y Agregar(...) recibe un UsuarioDto.
Por dentro el servicio sigue guardando entidades Usuario y convierte con UsuarioMapper.
Restricciones: no toques LabDTO.Web. Es esperable que la página deje de compilar:
no la arregles todavía.
Criterio de aceptación: "dotnet build LabDTO.BusinessLogic" compila sin errores.
```

> **💡 Pista**
> La restricción «no toques LabDTO.Web» es importante: si no la ponen, es muy probable que el agente vea que la página no compila y la arregle por su cuenta, salteándose lo que queremos que lean.

**Revisar y verificar.** Leé el diff de `UsuarioServicio.cs` y después:

```sh
# Terminal
dotnet build                          # falla: la página ya no compila
dotnet build LabDTO.BusinessLogic     # compila
```

Lean los errores de la página y comparen con la predicción.

Commit a mano.

> **✅ Checkpoint**
> `LabDTO.BusinessLogic` compila y `LabDTO.Web` no. Los errores señalan exactamente los lugares donde la página usaba la entidad.

> **🤔 Para pensar**
> Esos errores te muestran de qué dependía el frontend. ¿Cuáles de esas dependencias eran necesarias y cuáles no?

---

### Ciclo C. Arreglar la página (10 min)

**Predecir.** Antes de pedir: ¿qué cambios va a tener que hacer el agente en `Usuarios.razor`? Anótenlos en una lista.

**Pedido, ahora con criterio de aceptación explícito:**

```
Arreglá @LabDTO.Web/Components/Pages/Usuarios.razor para que trabaje con UsuarioDto.
Objetivo: la tabla muestra Id, Nombre y Email, sin columna de contraseña, y el alta
arma un UsuarioDto en vez de una entidad. La página no decide ninguna contraseña.
Restricciones: no uses el mapper en la página (la conversión la hace el servicio).
No toques otros proyectos.
Criterio de aceptación: la página no tiene "@using LabDTO.Domain" ni nombra
la entidad "Usuario" en ningún lado, y "dotnet build" compila toda la solución.
```

**Revisar el diff y verificar.**

| ¿Qué miro? | Cómo tiene que estar | ¿Está bien? |
|---|---|---|
| `@using` de la página | El de la carpeta de DTOs, no el de Domain | |
| Tabla | Id, Nombre y Email; sin contraseña | |
| Alta | Arma un `UsuarioDto`; no asigna contraseña | |
| Mapper en la página | No aparece | |

```sh
# Terminal
dotnet build   # toda la solución tiene que compilar
```

Después ■ **Stop** y ▶ **Run** sobre `LabDTO.Web`, y prueben listar y dar de alta un usuario.

Commit a mano.

> **✅ Checkpoint**
> La solución compila entera, la tabla muestra Id, Nombre y Email, el alta funciona y hay tres commits hechos por ustedes (uno por ciclo).

---

## Paso 4. Cierre y verificación

*10 min*

### 4.1 Chequeo de arquitectura (2 min)

Buscá en el proyecto Web cualquier mención del dominio. **No tiene que aparecer nada.**

```sh
# macOS / Linux / Git Bash
grep -rn "LabDTO.Domain" LabDTO.Web --include=*.razor --include=*.cs
grep -rnw "Usuario" LabDTO.Web --include=*.razor --include=*.cs
```

```powershell
# PowerShell
Get-ChildItem LabDTO.Web -Recurse -Include *.razor,*.cs | Select-String "LabDTO.Domain"
Get-ChildItem LabDTO.Web -Recurse -Include *.razor,*.cs | Select-String "\bUsuario\b"
```

(La segunda búsqueda usa palabra completa, así que `UsuarioDto` no cuenta.)

Si encuentran algo, pídanle al agente que lo corrija (con un pedido preciso) y vuelvan a commitear.

Antes de terminar, probá escribir `@u.Password` en alguna celda de la tabla y mirá qué marca Rider. Después sacalo **sin commitear**.

### 4.2 Prueba en el navegador (3 min)

Con la app corriendo, listen y agreguen un usuario. La contraseña no tiene que verse en ningún lado.

### 4.3 Tres reglas para el agente (5 min)

Hoy el agente solo conocía la arquitectura y los comandos; no sabía qué es un DTO ni dónde va cada cosa en este proyecto. Con lo que tuvieron que corregir, escriban **tres reglas** en `AGENTS.md` para que el próximo pedido no tenga esos errores. Por ejemplo:

```
- Las entidades de Domain no salen de BusinessLogic: los servicios reciben y devuelven DTOs.
- Un DTO es una clase sin lógica ni referencias a Domain, y no incluye contraseñas.
```

Escribanlas con **sus propias palabras y a partir de sus errores**, no copien las del ejemplo. Hagan un último commit a mano.

> **✅ Checkpoint final**
> El chequeo de arquitectura no encuentra nada, la app funciona y `AGENTS.md` tiene tres reglas nuevas, escritas por ustedes, con su commit.

> **🤔 Para pensar (cierre oral, 2 min)**
> - Si ahora renombran `Email` a `Correo` en `Usuario`, ¿qué proyectos dejan de compilar? ¿Y la página?
> - Borraste el `@using`, pero Web sigue referenciando a BusinessLogic, que referencia a Domain. ¿Qué le impide a alguien (o a un agente) volver a usar `Usuario` en la página? ¿Qué cosa lo detecta?
> - ¿Qué habría pasado si commiteaban sin leer el diff?

### Registro del grupo (para entregar)

| | |
|---|---|
| ¿Qué hizo bien el agente sin que se lo pidieran? | |
| ¿Qué tuvieron que corregir? ¿Cómo lo detectaron? | |
| Una diferencia entre el pedido corto y el preciso | |
| Las tres reglas que agregaron a `AGENTS.md` | |

---

## 🏁 Vuelta extra (de tarea)

*para los que quieran seguir*

**1. Un DTO para el alta.** ¿Tiene sentido que el formulario use un DTO con `Id`, si el Id lo pone el servicio? Pidanle al agente un `UsuarioAltaDto` solo con lo que se carga en el formulario, y su método en el mapper. Escriban el pedido con objetivo, restricciones y criterio de aceptación, y revisen el diff antes de commitear.

**2. Una página nueva.** Agregá una página *Directorio* que muestre solo los nombres, con su propio DTO y su opción en el menú.

**3. Investigá otro agente.** Lo que hiciste hoy con OpenCode no es particular de esa herramienta: archivo de instrucciones del proyecto, plan antes de editar, permisos y revisión del diff existen en otros agentes de consola, como **Claude Code** o **Codex**. Elegí uno, buscá su documentación oficial, instalalo y repetí el Ciclo C o la vuelta extra 1. Entregá una nota corta con tres puntos:

- ¿Cómo se llama su archivo de instrucciones del proyecto y qué contiene el tuyo?
- ¿Cómo se controlan los permisos (por ejemplo, impedir que haga commits)?
- ¿Qué diferencias notaste en el diff que produjo respecto de OpenCode?

> **💡 Pista**
> Hasta donde sabemos, Claude Code lee `CLAUDE.md` y Codex lee `AGENTS.md`; OpenCode v2 lee solamente `AGENTS.md`. Verificalo en la documentación de cada uno, porque estas herramientas cambian rápido. Parte del ejercicio es aprender a leer esa documentación.

---

*Capas, entidad de dominio, DTO y mapper a mano · trabajo con agente de consola (OpenCode v2) · Blazor con Rider y .NET 9. Ejercicio guiado de 60 minutos.*

---
---

# ANEXO DOCENTE (sacar esta parte antes de repartir el práctico)

## A. Preparación antes de clase

- [ ] **OpenCode v2 instalado y conectado en todas las máquinas**, probado el día anterior (`opencode --version` empieza con 2, y una pregunta simple recibe respuesta).
- [ ] **Proveedor decidido de antemano** (clave institucional u otra opción) y probado desde una máquina de alumno. La guía oficial menciona OpenCode Console y OpenCode Go (suscripción paga) para quienes empiezan.
- [ ] **Repo git** con `.gitignore` de .NET (`bin/`, `obj/`, `.idea/`, `.vs/`), commit inicial limpio y rama `ejercicio-agente`.
- [ ] **`AGENTS.md` base** (sección B) y **`opencode.jsonc`** (sección C) en la raíz.
- [ ] **Tests de humo** (ver nota 4).
- [ ] **Un diff de referencia por ciclo**, por si un grupo se traba.
- [ ] Si no pueden instalar en clase: pedir el Paso 0 de tarea el día anterior, con el checkpoint como comprobante.

## B. `AGENTS.md` base

Este archivo **a propósito no incluye reglas sobre DTOs**. Si las incluyera, el agente no cometería los errores del Ciclo A y los alumnos escribirían las reglas del cierre sin haber sufrido su ausencia.

```markdown
# LabDTO: instrucciones para el agente

## Arquitectura
Solución .NET 9 en tres capas. Cada flecha es una referencia de proyecto:

LabDTO.Web  ->  LabDTO.BusinessLogic  ->  LabDTO.Domain

- `LabDTO.Domain`: entidades (por ejemplo `Usuario`).
- `LabDTO.BusinessLogic`: servicios (por ejemplo `UsuarioServicio`).
- `LabDTO.Web`: aplicación Blazor (páginas en `Components/Pages`).

## Comandos
- Compilar: `dotnet build`
- Tests: `dotnet test`
- Correr la app: `dotnet run --project LabDTO.Web` (queda en http://localhost:5080)

## Forma de trabajo
- Hacé cambios chicos y limitados a lo que se pide.
- No hagas `git commit` ni `git push`: los commits los hace una persona desde Rider.
- No modifiques proyectos que el pedido no mencione.
```

## C. `opencode.jsonc`

Formato de permisos de la v2: una lista ordenada de reglas, y **gana la última que coincide**. Por eso la regla general va primero y las excepciones después.

```jsonc
{
  "$schema": "https://opencode.ai/config.json",
  "permissions": [
    // Por defecto, el agente pregunta antes de ejecutar cualquier comando
    { "action": "shell", "resource": "*", "effect": "ask" },

    // Compilar, testear e inspeccionar git: permitido sin preguntar
    { "action": "shell", "resource": "dotnet build *", "effect": "allow" },
    { "action": "shell", "resource": "dotnet test *", "effect": "allow" },
    { "action": "shell", "resource": "git status *", "effect": "allow" },
    { "action": "shell", "resource": "git diff *", "effect": "allow" },
    { "action": "shell", "resource": "git log *", "effect": "allow" },

    // Los commits y todo lo que reescribe historia o cambia de rama lo hacen las personas
    { "action": "shell", "resource": "git commit *", "effect": "deny" },
    { "action": "shell", "resource": "git push *", "effect": "deny" },
    { "action": "shell", "resource": "git reset *", "effect": "deny" },
    { "action": "shell", "resource": "git checkout *", "effect": "deny" },
    { "action": "shell", "resource": "git switch *", "effect": "deny" },
    { "action": "shell", "resource": "git rebase *", "effect": "deny" },
    { "action": "shell", "resource": "git merge *", "effect": "deny" },
    { "action": "shell", "resource": "git stash *", "effect": "deny" },
    { "action": "shell", "resource": "git clean *", "effect": "deny" }
  ]
}
```

La edición de archivos queda permitida por defecto para el agente Build, y el agente Plan no puede editar archivos normales del proyecto. El diff en Rider es el control de verdad sobre lo editado.

## D. Notas y advertencias

1. **Versión 2.** La guía fue armada contra la documentación de OpenCode v2 (https://opencode.ai/v2/docs/). Cambios respecto de la v1 que ya están incorporados: instalador `https://opencode.ai/v2/install`, formato `permissions` con `shell` (en v1 era `permission` con `bash`), cambio de agente con **Shift+Tab** (en v1 era Tab) y solo `AGENTS.md` como archivo de instrucciones (no hay fallback a `CLAUDE.md`).
2. **Qué no pude verificar y conviene probar antes de clase:**
   - Los comandos exactos de **npm, Homebrew, Bun, pnpm, Yarn y AUR para la v2**: la página oficial los muestra en pestañas que no pude leer, por eso el práctico manda a copiarlos de ahí. El README de GitHub muestra comandos que pueden instalar la v1.
   - La instalación en **Windows nativo** (descomprimir el zip y agregar al `PATH`): la documentación solo ofrece los binarios, sin pasos. Si dan problemas, WSL con el script es la alternativa.
   - Que **`/init`** exista en la v2: la v1 lo documentaba, pero no lo encontré en las páginas de la v2 que leí. Por eso el práctico trae el `AGENTS.md` ya armado y no usa `/init`.
   - El **renderizado de la interfaz en la terminal de Rider** (la documentación sugiere Ghostty, WezTerm, Alacritty o Kitty). `opencode mini` queda como alternativa.
   - Las referencias a Claude Code y Codex (qué archivo lee cada uno) están dadas de memoria y marcadas como tales en la vuelta extra.
3. **La trampa del Ciclo A no está garantizada.** Con un pedido corto, un buen modelo puede devolver un DTO impecable. Si pasa, la lección cambia: el grupo igual tiene que demostrar con la lista del diff que verificó, y el contraste pedido corto/preciso se mira en otros aspectos (carpetas, tocó o no la página). Para forzar el error, la variante más fiable es el pedido «hacé un DTO de usuario con todas las propiedades de la entidad», que casi seguro incluye `Password`.
4. **Los tests de humo se rompen en el Ciclo B.** Cualquier test que llame a `Agregar` con una entidad deja de compilar cuando la firma pasa a recibir un `UsuarioDto`. Dos opciones: que los tests prueben solo `Listar()` y cuenten elementos (no se rompen), o sumar al pedido del Ciclo B «y actualizá los tests», lo que agrega un minuto pero sirve para enseñar que el agente también ajusta tests. En el Ciclo B se compila solo `LabDTO.BusinessLogic` y no la solución entera, así que si el proyecto de tests referencia solo BusinessLogic, `dotnet test` sigue funcionando.
5. **Las reglas `deny` de git son una barrera, no una garantía.** La propia documentación advierte que conviene una lista permitida acotada antes que intentar reconocer todos los comandos peligrosos, y que `shell` corre con la autoridad del usuario. El control real es revisar el diff y commitear a mano. Se puede mencionar en clase.
6. **`/undo` y el repo.** `/undo` restaura archivos solo dentro de un repositorio git, no cubre archivos ignorados por git ni deshace commits, y puede sobrescribir ediciones recientes de los mismos archivos. Se aclara en el Paso 0 como red de seguridad, no como reemplazo del commit.
7. **Chequeo de arquitectura.** El práctico usa una búsqueda de texto (`grep` / `Select-String`) para no depender de un test extra. Si querés automatizarlo, es un buen candidato para un test que falle si `LabDTO.Web` menciona `LabDTO.Domain`. Aclaración para la discusión final: como Web referencia a BusinessLogic y esta a Domain, `Usuario` sigue siendo alcanzable desde Web (la referencia es transitiva); lo que lo detecta es el chequeo, no el compilador.
8. **Tiempo.** Los 60 minutos asumen que OpenCode ya está instalado y conectado. Con instalación en clase, calculá 10 minutos más o pasalo a tarea previa.