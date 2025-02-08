<a name='assembly'></a>
# FarlandsCoreMod

## Contents

- [CommandShell](#T-CommandTerminal-CommandShell 'CommandTerminal.CommandShell')
  - [RegisterCommands()](#M-CommandTerminal-CommandShell-RegisterCommands 'CommandTerminal.CommandShell.RegisterCommands')
  - [RunCommand()](#M-CommandTerminal-CommandShell-RunCommand-System-String- 'CommandTerminal.CommandShell.RunCommand(System.String)')
- [Resources](#T-FarlandsCoreMod-Properties-Resources 'FarlandsCoreMod.Properties.Resources')
  - [Culture](#P-FarlandsCoreMod-Properties-Resources-Culture 'FarlandsCoreMod.Properties.Resources.Culture')
  - [ResourceManager](#P-FarlandsCoreMod-Properties-Resources-ResourceManager 'FarlandsCoreMod.Properties.Resources.ResourceManager')
  - [Version](#P-FarlandsCoreMod-Properties-Resources-Version 'FarlandsCoreMod.Properties.Resources.Version')
  - [fcm_scenes](#P-FarlandsCoreMod-Properties-Resources-fcm_scenes 'FarlandsCoreMod.Properties.Resources.fcm_scenes')

<a name='T-CommandTerminal-CommandShell'></a>
## CommandShell `type`

##### Namespace

CommandTerminal

<a name='M-CommandTerminal-CommandShell-RegisterCommands'></a>
### RegisterCommands() `method`

##### Summary

Uses reflection to find all RegisterCommand attributes
and adds them to the commands dictionary.

##### Parameters

This method has no parameters.

<a name='M-CommandTerminal-CommandShell-RunCommand-System-String-'></a>
### RunCommand() `method`

##### Summary

Parses an input line into a command and runs that command.

##### Parameters

This method has no parameters.

<a name='T-FarlandsCoreMod-Properties-Resources'></a>
## Resources `type`

##### Namespace

FarlandsCoreMod.Properties

##### Summary

Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.

<a name='P-FarlandsCoreMod-Properties-Resources-Culture'></a>
### Culture `property`

##### Summary

Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
  búsquedas de recursos mediante esta clase de recurso fuertemente tipado.

<a name='P-FarlandsCoreMod-Properties-Resources-ResourceManager'></a>
### ResourceManager `property`

##### Summary

Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.

<a name='P-FarlandsCoreMod-Properties-Resources-Version'></a>
### Version `property`

##### Summary

Busca una cadena traducida similar a 0.1.3.

<a name='P-FarlandsCoreMod-Properties-Resources-fcm_scenes'></a>
### fcm_scenes `property`

##### Summary

Busca un recurso adaptado de tipo System.Byte[].
