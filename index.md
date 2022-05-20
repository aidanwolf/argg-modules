# ModuleScript Documentation

## ModuleScript 101

- Modules can be nested within other modules `Module[onAction:Module2[onAction:Module3]]`

- Same modules cannot execute side-by-side:
  - ❌ `SetVar[variable:var1,value:true],SetVar[variable:var2,value:false]`
  - ✅ `SetVar[variable:var1,value:true,onVar:SetVar[variable:var2,value:false]]`

- Modules can be turned on or off by modules like [StateMachine](#statemachine)

- If no script is included, the default Inventory item behavior is `Item[onSpawn:Physical]`

- _Think of ModuleScript like a long strand of DNA that decides how an object behaves_

## Directory

[Modules](#modules)

[Variables](#variables-101)

[Position](#position-101)

## Modules

[Item](#item)

[Equip](#equip)

[Drop](#drop)

[Collect](#collect)

[StateMachine](#statemachine)

[SetState](#setstate)

[Gun](#gun)

[HealthSystem](#healthsystem)

[Anim](#anim)

[Unique](#unique)

[AnchorToHand](#anchortohand)

[Collidable](#collidable)

[Physical](#physical)

[Damage](#damage)

[Destroy](#destroy)

[Interactive](#interactive)

[Dialog](#dialog)

[Broadcast](#broadcast)

[LookAt](#lookat)

[Move](#move)

[NFC](#nfc)

[Scale](#scale)

[Spawn](#spawn)

[Timer](#timer)

[Trigger](#trigger)

[Console](#console)

[SetVar](#setvar)

[GetVar](#getvar)

[IfElse](#ifelse)

### SetVar
`SetVar[variable:string,value:(number || string),onVar:modulescript]`

Set a variable such as `highscore`

Use no prefix as in `variable:string` to set a temporary global variable (client-side)

Use `!` as in `variable:!string` to add a persistent variable to the player (server-side)

Use `#` as in `variable:#string` to set a persistent variable on the item (server-side)

### GetVar
`GetVar[variable:string,always:bool,onVar:modulescript]`

Gets the value of a variable and passes it on to `onVar`

Pass variable data thru onVar by including `v` anywhere in `onVar:modulescript` (example:`onVar:Console[message:var equals v]`)

Use `always:true` to make the module check for consistent updates, otherwise it will only check once

Note: Make sure to include the proper prefix `!` or `#` when interacting with server-side variables

### IfElse
`IfElse[variable:string,equals:||gt:||gte:||lt:||lte:||modulo:,always:bool,onTrue:modulescript,onFalse:modulescript]`

Checks if condition is met and passes through to `onTrue` and `onFalse`

Pass variable data thru onTrue and onFalse by including `v` (example:`onTrue:Console[message:v is correct!],onFalse:Console[message:v is wrong!]`)

`gt:(number or variable)` greater than

`gte:(number or variable)` greater than or equal to

`lt:(number or variable)` less than

`lte:(number or variable)` less than or equal to

`modulo:(number or variable)` leaves 0 remainder (same as 16 % 4 = 0)

Use `always:true` to make the module check for consistent updates, otherwise it will only check once

Note: Make sure to include the proper prefix `!` or `#` when interacting with server-side variables

### AnchorToHand
`AnchorToHand[slowParent:number,bothHands:true]`

Easily put an object in the player's hand

Good in combination with `Item[onEquip:]`

### Anim
`Anim[(play:string || loop:string || pingpong:string),speed:number,onAnimStart:modulescript,onAnimEnd:modulescript]`

Play an available animation by passing the animation name to play, loop, or pingpong

### Collidable
`Collidable[collider:string,onHit:modulescript]`

Make object collidable with other objects, react to collision events. Setting `collider` will find the child object of the same name to be collider

### Physical
`Physical[weight:number,gravity:bool,freezeRotation:bool,interactive:bool,onGrab:modulescript,onHit:modulescript,onHitGround:modulescript]`

Make an object react physically, drop with gravity, etc. i.e. rigidbody physics

### Damage
`Damage[damage:number,hitForce:number,hitRate:number,onDamage:modulescript]`

Make an object cause damage to other items

### Destroy
`Destroy[forReal:bool]`

Remove an object from the game world, or toggle forReal to remove it from the server

### Gun
`Gun[(projectile:uri || rayColor:vector4,rayTexture:uri,rayForce:number,damage:number),semiAuto:bool,shootAnim:string]`

All-in-one Gun module, by default acts like a raygun but can also shoot projectiles

You need to add a child object at the right position and rotation and name it **'BARREL'** to control where the projectile or ray comes out of

Automatically adds trigger button on screen when equipped

### HealthSystem
`HealthSystem[hp:number,damagedBy:stringList,onHPGain:modulescript,onHpLost:modulescript,onHpZero:modulescript]`

HealthSystem receives damage from damage dealing objects, by default onHpZero will remove the object from the game world

### Interactive
`Interactive[onTap:modulescript]`

Makes an object tappable. This module will be upgraded overtime

### Item
`Item[onSpawn:,onEquip:,onDrop:,onCollect:,onInspect:]`

Makes an object into an item which can be equipped, dropped, inspected, or collected into your inventory (behaves similarly to the StateMachine module)

### Equip
`Equip`

Interacts with Item module to trigger onEquip

### Drop
`Equip`

Interacts with Item module to trigger onDrop

### Collect
`Equip`

Instantiates a collect UI and interacts with Item module to trigger onCollect

### LookAt
`LookAt[target:unique,speed:number]`

Make an object look towards another object in the game world

### Move
`Move[(target:unique || position:vector3 || direction:vector3 ),(speed:number || time:number),impulse:bool,onMove:modulescript]`

Move an object in multiple ways. Use `target:` to move an object to or towards another, etc.

### NFC
`NFC[uri:uri,setVar:v,onDetect:modulescript]`

Make an object read an NFC and pass a variable `v` to all `v` instances in `onDetect:` ex:`onDetect:Console[message:v]`

### Scale
`Scale[scale:number]`

Set the scale of an object

### StateMachine
`StateMachine[state:(number or variable),state1:modulescript,state2:modulescript,...]`

Enable an object to hold multiple states (ex: enemy would have idle, patrol, follow, attack, defend, and death state)

### SetState
`SetState[to:(number || variable)]`

Set the state from within a StateMachine

### Spawn
`Spawn[uri:uri,position:vector3,rotation:vector4,time:number]`

Spawn another object. IMPORTANT: This will soon change to only objects the object owns (ex: zombie owns zombie meat)

### Timer
`Timer[time:number,onTime:modulescript]`

Classic old timer, make something happen when the time is up

### Trigger
`Trigger[label:string,onTrigger:modulescript]`

Adds trigger button on screen

### Unique
`Unique[id:unique]`

Make an object unique in the game world to be actionable to other items

Example: Carrot with `Unique[id:carrot]` can now be pursued by Bunny with `LookAt[target:carrot],Move[target:carrot,time:3]`

### Console
`Console[message:string]`

Output something on screen, good for debugging
You can use `<>` tags to display variables

### Broadcast
`Broadcast[message:string,tts:bool]`

Sends a message to the ARGG Discord server (currently to the #🐤-activity channel)

You can use `<>` tags to display variables

### Dialog
`Dialog[message:string]`

Adds a dialog box with message included. Text effects achieved with SuperTextMesh API
http://supertextmesh.com/docs/SuperTextMesh.html

You can use `<>` tags to display variables

## Variables 101

There will be a growing list of variables that developers can use to add deep functionality to their objects and games

**PLAYER_ID** - returns Discord user id

**PLAYER_TAG** - returns <@user id> (useful for tagging a user on Discord) `Broadcast[message<PLAYER_TAG> did a thing!`]

**GG** - coming soon

## Position 101

Spawn in world space `position:(0,0,0)` or `position:(0,0,0)`

Spawn in local space `position:+(0,0,0)` or `position:-(0,0,0)`

Create a new position `position:PLAYER+(0,1,0)`

### Position Shortcuts

**PLAYER** - position of player's device `position:PLAYER+(0,0,1)`

**HAND_ANCHOR** - hand position `position:HAND_ANCHOR`

**PLAYER_VIEW** - 1M in front of player's device (this will change `position:PLAYER_VIEW`

_WALL_IN_VIEW - coming soon_

_GROUND_IN_VIEW - coming soon_

_CEILING_IN_VIEW - coming soon_

_NEAREST_WALL - coming soon_

_NEAREST_FLOOR - coming soon_

_NEAREST_GRASS - coming soon_

_COFFEE_MAKER - coming soon_

