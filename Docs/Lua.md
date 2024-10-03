## Funciones Principales

### MOD(name)
Inicia el mod con el nombre indicado, debe ser ejecutado en la primera l�nea

### **add_language(path)
Agrega un idioma dada la ruta a su archivo `.json`

### add_command(name, luaFunc, help)
Agrega un comando de consola

### add_credits(quantity)
Agrega una cantidad de cr�ditos

### add_item(_id, quantity)
Agrega un item a tu inventario

### create_scene(name)
Crea una nueva escena

### create_object(name)
Crea y devuelve un nuevo objeto con el nombre indicado

### config(section, key, default, description)
Agrega una configuraci�n

### create_inventory_item(item)
Crea un nuevo item para inventarios
Parametros: {name, itemType, spritePath, buyPrice, sellPrice, canBeStacked, canBeDestroyed, matterPercent}
```lua
item = {
	name = , -- nombre 
	
	-- puede tomar los siguientes valores 'RESOURCE', 'TOOL', 'SEED', 'CRAFTING', 'FISH', 'INSECT', 'PLACEABLE'
	type = , -- tipo
	sprite = , -- ruta del sprite
	buy_price = , -- precio de compra
	sell_price = , -- precio de venta
	stackable = , -- si puede acumularse en un �nico slot de inventario
	destroyable = , -- si puede ser destruido
	matter_percent = , cantidad de combustible que le da a la nave
}
```
### create_plant
Crea un nuevo item para inventarios
Parametros: {name, days_for_death, days_for_stage, grow_season, resources, seed, stage_1, stage_2, stage_3, stage_4, stage_5}
```lua
item = {
	name = , -- nombre 
	days_for_death = , -- dias que puede sobrevivir sin regar
	days_for_stage= , -- dias para cambio de estado
	grow_season = , -- temporada en la que crece (tenemos que comprobar los valores)
	resources = , -- recursos que puede dar
	seed = , -- sprite para la semilla
	stage_n = , -- donde n es un n�mero del 1 al 5, sprite para el estado correspondiente
}
```
### create_seed(inventoryId, plantsId)
Crea una semilla asignando un objeto de inventario a una lista de plantas

### execute_command(command)
Ejecuta un comando

### find_object(name)
Devuelve un objeto de la escena con nuevas funciones

### get_input(action)
Obtiene si un input se est� pulsando

### get_object(path)
Obtiene un objeto especificando la ruta de objetos

### load_scene(scene)
Carga una escena indicada

### potrait_override(origin, path)
Cambia la textura de un portrait por la textura indicada con el `path`

### print(text)
Muesta por terminal el mensaje indicado

### print_scene()
Muesta por terminal la escena actual

### texture_override(path)
Cambia la textura indicada con el nombre del archivo `path`

### texture_override(origin, path)
Cambia la textura origin por la textura indicada con el `path`

### texture_override_in(path)
Cambia todas las texturas de una carpeta indicadas con el nombre de cada archivo

### toggle_ui()
Desactiva la interfaz de usuario **OCASIONA ERRORES**

### translate_inventory_item(id, names, descriptions)
Agrega traducciones a un inventory items

## Funciones matem�ticas
Funciones que tienen que ver con matem�ticas, estas funciones se encuentran detro de math, por ejemplo
`math.normalize(vector)`
### normalize(vector)
Normaliza un vector

## Lua GameObject

### add_component(component)
Agrega el componente indicado al objeto

### add_position(x, y, z)
Agrega posici�n al objeto

### get_language()
Devuelve el idioma actual

### get_position()
Devuelve la posici�n del objeto

### set_position(x, y, z)
Cambia la posici�n del objeto

### toggle_active()
Cambia su estado activo