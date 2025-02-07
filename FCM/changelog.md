# Version 0.2.0.4
## FarlandsCoreMod
- Se ha arreglado un error en la versión.
- Se ha agregado la versión en el menú principal.

### UIMaker
- Se ha creado la clase `RButton`
- Se ha cambiado el logo de FM para que sea un botón.

### AssetBundle
- Se ha agregado el sprite `FarlandsModLogo`.
- Se ha agregado el sprite `FarlandsModLogo32x32`.
- Se ha agregado el sprite `FarlandsModLogo24x24`.
- Se ha agregado el sprite `UI_FM`.

# Versiones 0.2.0.0 ~ 0.2.0.3
- Cambio en la estructura del proyecto.
- Creación de **FCM Test**.

## FarlandsCoreMod
- Se han eliminado todos los scripts.
- se ha creado un _AssetBundle_ para **FCM**.
- se ha creado la interfaz `ISpriteLoader`.
- ahora los mods se identifican usando su _GUID_.
- se ha cambiado el _GUID_ de **FCM** a `magin.fcm`.
- la clase `FarlandsCoreMod` ahora implementa de `ISpriteLoader`.
- se ha creado la clase `SpriteManager`.
- se ha creado un paquete para la gestión de la interfaz gráfica.

### UIMaker
- se ha creado la clase `UIMaker`.
- se ha creado la interfaz `IRenderizable`.
- se ha creado la clase `RElement` como base para el resto.
- se ha creado la clase `RCanvas`.
- se ha creado la clase `RNone`.
- se ha creado la clase `RRect`.
- se ha creado la clase `RImage`, hereda de `RRect`.
- se ha creado la clase `RButton`, hereda de `RImage`.

## FCM Test
- se requiere una variable de entorno llamada *FARLANDS_PATH*
- se ha creado la clase `Program`, para iniciar el juego.
- se ha agregado opción debug para poder ser utilizado junto con dnSpy.