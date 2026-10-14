# Ejercicio DTO de Usuarios

*Blazor · Ejercicio guiado · 45 min*

Te damos una aplicación Blazor separada en tres capas (Domain, BusinessLogic y Web) que ya funciona: lista usuarios y permite dar de alta nuevos. Tu trabajo es leer el código, encontrar qué está mal en cómo el frontend usa los datos, y corregirlo con un DTO y un mapper.

`análisis de código` · `capas` · `entidad de dominio` · `DTO` · `mapper` · `desacoplar el frontend` · `ocultar la contraseña`

---

## Antes de arrancar

- **Rider** instalado y un navegador.
- El **SDK de .NET 9** (con .NET 10 también funciona).
- La solución inicial `LabDTO-inicial.zip` que te pasa el docente.
- Tiempo: **45 min**, en grupos de 3 o en parejas. Al final el frontend ya no conoce la entidad de dominio: BusinessLogic le entrega DTOs, y un mapper hace la traducción en los dos sentidos.

```sh
# Terminal
dotnet --version   # tiene que empezar con 9
```

---

## Paso 0. Abrir la solución

*LabDTO.sln · tres proyectos*

Descomprimí `LabDTO-inicial.zip`. En Rider elegí **File → Open** y abrí el archivo `LabDTO.sln`.

En el panel de la izquierda vas a ver tres proyectos, uno por capa. Arriba a la derecha, en la lista de configuraciones de ejecución, elegí **LabDTO.Web** y dale **▶ Run**. La app queda en una dirección fija:

```
http://localhost:5080
```

> **✅ Checkpoint**
> En el navegador ves la Home de **LabDTO**, con un menú lateral que tiene **Home** y **Usuarios**. Si Rider no abrió el navegador solo, copiá la dirección en una pestaña nueva.

---

## Paso 1. Recorrer la aplicación y el código

*antes de tocar nada, entender qué hay*

Entrá a **Usuarios** desde el menú. Mirá la tabla y agregá un usuario con el formulario de abajo.

La solución está separada en capas. Cada flecha es una referencia entre proyectos: el de la izquierda puede usar las clases del de la derecha.

```
LabDTO.Web  ──▶  LabDTO.BusinessLogic  ──▶  LabDTO.Domain
(frontend)       (lógica de negocio)         (entidades)

LabDTO.Domain/Usuario.cs                       la entidad de dominio
LabDTO.BusinessLogic/UsuarioServicio.cs        lista y agrega usuarios
LabDTO.Web/Components/Pages/Usuarios.razor     la página que acabás de usar
```

Abrí esos tres archivos en Rider y leelos con calma. Después respondan estas preguntas. No hace falta que estén seguros: escriban lo que piensan.

| Pregunta | Respuesta del grupo |
|---|---|
| ¿Qué datos de cada usuario muestra la página? ¿Alguno no debería verse? | |
| En `Usuarios.razor`, ¿qué `@using` hay arriba? ¿De qué proyecto viene la clase que recorre la tabla? | |
| El proyecto Web solo referencia a BusinessLogic. ¿Cómo puede entonces usar una clase de Domain? | |
| Cuando agregás un usuario, ¿quién decide su contraseña? ¿En qué capa está esa decisión? | |
| Si mañana en `Usuario` renombran `Email` a `Correo`, ¿qué proyectos dejan de compilar? | |

> **🤔 Para pensar**
> La solución está separada en capas, pero la página trabaja directo con la entidad de dominio. ¿Para qué sirve separar en proyectos si el frontend termina conociendo todo igual?

---

## Paso 2. Crear el DTO

*en BusinessLogic, con solo lo que el frontend necesita*

Click derecho sobre el proyecto `LabDTO.BusinessLogic` → **Add → Directory**, nombre `Dtos`. Dentro de esa carpeta, **Add → Class/Interface**, nombre `UsuarioDto`. Reemplazá el contenido por esto y resolvé el **TODO 1**:

```cs
// LabDTO.BusinessLogic/Dtos/UsuarioDto.cs
namespace LabDTO.BusinessLogic.Dtos;

// TODO 1: agregá las propiedades que el frontend necesita.
// Usá las respuestas del paso 1 para decidir qué entra y qué queda afuera.
public class UsuarioDto
{
}
```

> **💡 Pista**
> Un DTO (Data Transfer Object) es una clase sin lógica: solo propiedades, pensada para llevar datos de una capa a otra. No hereda de la entidad ni la referencia: es una clase aparte, con las propiedades que vos elijas.
>
> En este ejercicio lo vamos a usar como el «formato de intercambio» entre BusinessLogic y Web: el servicio va a entregar y recibir DTOs, y la entidad `Usuario` se queda adentro de BusinessLogic. Eso nos sirve para dos cosas:
>
> **Desacoplar capas.** El frontend depende solo del DTO, no de cómo está armada la entidad de dominio. Si mañana la entidad cambia (se renombra una propiedad, se agrega un campo interno), alcanza con ajustar la traducción en BusinessLogic: la página no se entera.
>
> **Ocultar información.** El DTO lleva solo lo que la otra capa necesita. Un dato que no está en el DTO no llega al frontend, ni por error: no hay forma de mostrarlo porque no existe en la clase que recibe la página.

```sh
# Terminal
dotnet build   # tiene que terminar en "Build succeeded."
```

> **✅ Checkpoint**
> Compila sin errores. Tu DTO tiene lo que la tabla y el formulario necesitan, y la contraseña quedó afuera.

---

## Paso 3. Crear el mapper

*traducir entidad ⇄ DTO, a mano*

Ahora tenemos dos clases que representan a un usuario: la entidad `Usuario`, que usa BusinessLogic por dentro, y el `UsuarioDto`, que es lo que viaja hacia el frontend. Alguien tiene que encargarse de pasar los datos de una a la otra, en los dos sentidos: de entidad a DTO cuando el servicio entrega usuarios, y de DTO a entidad cuando recibe uno nuevo.

Esa responsabilidad no le corresponde al DTO. Como vimos, el DTO es solo un contenedor de datos, sin lógica: si le agregáramos métodos para convertirse, tendría que conocer la entidad de dominio, y eso es justo lo que queremos evitar. Tampoco le corresponde a la página, que no debería saber que la entidad existe. Por eso la ponemos en una clase aparte, dedicada solo a eso: el **mapper**. Así toda la traducción entre las dos clases queda en un único lugar.

En `LabDTO.BusinessLogic` creá la carpeta `Mappers` y adentro la clase `UsuarioMapper`. Reemplazá el contenido por esto y resolvé los **TODO 2** y **3**:

```cs
// LabDTO.BusinessLogic/Mappers/UsuarioMapper.cs
using LabDTO.BusinessLogic.Dtos;
using LabDTO.Domain;

namespace LabDTO.BusinessLogic.Mappers;

public static class UsuarioMapper
{
    public static UsuarioDto ToDto(Usuario usuario)
    {
        // TODO 2: devolvé un UsuarioDto con los datos de "usuario"
    }

    public static Usuario ToEntity(UsuarioDto dto)
    {
        // TODO 3: devolvé un Usuario armado con los datos de "dto".
        // El DTO no trae contraseña: buscá en el código actual qué contraseña
        // reciben los usuarios nuevos y mové esa decisión acá.
    }
}
```

> **💡 Pista**
> Para armar un objeto nuevo copiando datos de otro se usa un object initializer: `new Tipo { Propiedad = origen.Propiedad, ... }`. Los métodos de una clase `static` se llaman con el nombre de la clase, sin crear una instancia.

```sh
# Terminal
dotnet build   # tiene que terminar en "Build succeeded."
```

> **✅ Checkpoint**
> Compila sin errores. El mapper vive en BusinessLogic, que es la única capa que conoce las dos clases: `Usuario` y `UsuarioDto`.

---

## Paso 4. El servicio habla en DTOs

*BusinessLogic deja de entregar la entidad*

Abrí `UsuarioServicio.cs`. Por dentro el servicio sigue guardando entidades `Usuario`, pero hacia afuera tiene que entregar y recibir DTOs.

- **TODO 4:** que `Listar()` devuelva una lista de `UsuarioDto`, convirtiendo cada usuario con el mapper.
- **TODO 5:** que `Agregar(...)` reciba un `UsuarioDto` y lo convierta a entidad antes de guardarlo.

> **💡 Pista**
> `.Select(...)` transforma cada elemento de una lista en otra cosa: le podés pasar directamente un método que reciba un elemento y devuelva el nuevo. Para que el resultado sea una lista, terminá con `.ToList()`.

```sh
# Terminal
dotnet build
```

> **✅ Checkpoint**
> `LabDTO.BusinessLogic` compila, pero `LabDTO.Web` **no**: la página falla en los lugares donde usaba la entidad. Leé los errores antes de seguir.

> **🤔 Para pensar**
> Los errores de compilación de la página te están mostrando exactamente de qué dependía el frontend. ¿Cuáles de esas dependencias eran necesarias y cuáles no?

---

## Paso 5. Arreglar la página

*el frontend deja de conocer el dominio*

Abrí `Usuarios.razor`. **TODO 6:** hacé que compile de nuevo trabajando con `UsuarioDto`: la tabla sin columna de contraseña, y el alta armando un DTO en vez de una entidad. La página no tiene que decidir ninguna contraseña.

> **💡 Pista**
> Cambiá el `@using` del dominio por el de la carpeta de DTOs. La página ya no necesita el mapper: la conversión ahora la hace el servicio.

Antes de terminar, probá escribir `@u.Password` en alguna celda de la tabla y mirá qué marca Rider. Después sacalo. **■ Stop** y **▶ Run** para probar la página.

> **✅ Checkpoint**
> La solución compila entera. La tabla muestra Id, Nombre y Email, el alta funciona, y `Usuarios.razor` ya no tiene `@using LabDTO.Domain` ni nombra la entidad `Usuario` en ningún lado.

> **🤔 Para pensar**
> Volvé a la última pregunta del paso 1: si ahora renombran `Email` a `Correo` en `Usuario`, ¿qué proyectos dejan de compilar? Y otra: borraste el `@using`, pero Web sigue referenciando a BusinessLogic, que referencia a Domain. ¿Qué le impide a alguien volver a usar `Usuario` en la página?

---

## 🏁 Vuelta extra (opcional)

*para los que terminaron antes*

- ¿Tiene sentido que el formulario de alta use un DTO con `Id`, si el Id lo pone el servicio? Creá un `UsuarioAltaDto` solo con lo que se carga en el formulario, y su método en el mapper.
- Agregá una página *Directorio* que muestre solo los nombres, con su propio DTO y su opción en el menú.

---

*Capas, entidad de dominio y DTO · mapper a mano · Blazor con Rider y .NET 9. Ejercicio guiado de 45 minutos.*
