# ModuleScript Documentation

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

[LookAt](#lookat)

[Move](#move)

[NFC](#nfc)

[Scale](#scale)

[Spawn](#spawn)

[Timer](#timer)

[Trigger](#trigger)

[Console](#console)

### AnchorToHand
`AnchorToHand[slowParent:number,bothHands:true]`

Easily put an object in the player's hand

Good in combination with `Item[onEquip:]`

### Anim
`Anim[(play:string || loop:string || pingpong:string),speed:number,onAnimStart:modulescript,onAnimEnd:modulescript]`

Play an available animation by passing the animation name to play, loop, or pingpong

### Collidable
`Collidable[]`

Make object collidable with other objects, react to collision events

### Physical
`Physical[weight:number,gravity:bool,freezeRotation:bool]`

Make an object react physically, drop with gravity, etc. i.e. rigidbody physics

### Damage
`Damage[damage:number,hitRate:number,onDamage:modulescript]`

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
`StateMachine[state:number,state1:modulescript,state2:modulescript,...]`

Enable an object to hold multiple states (ex: enemy would have idle, patrol, follow, attack, defend, and death state)

### SetState
`SetState[to:number]`

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
