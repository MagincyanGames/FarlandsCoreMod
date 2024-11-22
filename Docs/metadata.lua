---@meta farlands
---@class Object
---@field get fun(key:string):any
---@field set fun(key:string, value:any):any
---@field call fun(function:string, value:any):any
---@class GameObject
---@field get fun(key:string):any
---@field set fun(key:string, value:any):any
---@field call fun(function:string, value:any):any
---@field get_name fun():string
---@field get_position fun():any
---@field set_position fun(pos:any):any
---@field add_position fun(pos:any):any
---@field set_scale fun(scale:any):any
---@field toggle_active fun():any
---@field get_layer fun():integer
---@field set_layer fun(id:integer):integer
---@field set_update fun(f:function):any
---@field set_start fun(f:function):any
---@meta farlands
---@class scenes
---@field load_scene fun(scene:any):any
---@field print_scene fun():any
scenes = {}
---@param tag string
function MOD(tag) end
---@param section string
---@param key string
---@param def any
---@param description string
function config(section,key,def,description) end
---@param input string
---@return any
function get_input(input) end
---@param _comando string
function execute_command(_comando) end
---@param arg0 any
---@param arg1 any
function texture_override(arg0,arg1) end
---@param path string
function texture_override_in(path) end
---@param origin string
---@param position integer[]
---@param path string
function sprite_override(origin,position,path) end
---@param origin string
---@param path string
function portrait_override(origin,path) end
---@param path string
function add_language(path) end
---@return string
function get_language() end
---@param txt string
function print(txt) end
---@param args any[]
---@return GameObject
function get_object(args) end
---@param name string
---@param scene any
---@return GameObject
function find_object(name,scene) end
---@param id any
---@param amount integer
function add_item(id,amount) end
---@param amount integer
function add_credits(amount) end
---@param name string
---@return any
function create_object(name) end
---@param args any
---@return integer
function create_inventory_item(args) end
---@param args any
---@return integer
function create_plant(args) end
---@param inventoryId integer
---@param plantsId any[]
function create_seed(inventoryId,plantsId) end
---@param id integer
---@param name any[]
---@param description any[]
function translate_inventory_item(id,name,description) end
---@param name string
function create_scene(name) end
---@param name string
---@param LuaFunc any
---@param help string
function add_command(name,LuaFunc,help) end
---@param origen any
---@param direction any
---@param max number
---@param _mascara integer
---@param user any
---@return any
function rayCast(origen,direction,max,_mascara,user) end
---@param layerName string
---@return integer
function get_layer_number(layerName) end
---@param assembly string
---@param className string
---@param method string
---@return any
function CSF(assembly,className,method) end