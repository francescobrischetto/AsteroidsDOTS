# The Sandbox | Unity Dev Test Documentation
**Francesco Brischetto**
_Unity Game Engineer @ Bevium_

`GitHub Prototype Repository`: https://github.com/francescobrischetto/AsteroidsDOTS

## Document Sections

1. [**Introduction**](#1-introduction)
2. [**Tools/Tech Stack Used**](#2-toolstech-stack-used)
3. [**Game Inputs**](#3-game-inputs)
4. [**Prototype Game Mechanics and Design Decisions**](#4-prototype-game-mechanics-and-design-decisions)
5. [**Fine-Tuning Game Values and Sounds**](#5-fine-tuning-game-values-and-sounds)
6. [**Script Folder Structure**](#6-script-folder-structure)
7. [**Implementation Details**](#7-implementation-details)
8. [**Possible Improvements**](#8-possible-improvements)

## 1. Introduction
The purpose of this document is to provide a detailed **overview of the developed game prototype** inspired by the classical _Asteroids_ . It includes information about **technology stack**, **game mechanics**, **design decisions** and **implementation details**.

## 2. Tools/Tech Stack Used
- Unity Editor : `2020.2.7f1`
	_Main packages_
	-  Entities package: `0.17.0-preview.42`
	- DOTS Editor: `0.12.0-preview.6`
	- Hybrid Renderer: `0.11.0-preview.44`
	- Unity Physics: `0.6.0-preview.3`
- Git
- "Space Shooter Redux" assets from Kenney.nl
- [8-bit-style-sound-effects](https://assetstore.unity.com/packages/audio/sound-fx/8-bit-style-sound-effects-68228#content) sound assets from cabled_mess

## 3. Game Inputs
The input actions available for the prototype are:
|Input|Key|
|:--|:--|
|Rotate Ship Left| `A`|
|Rotate Ship Right| `D`|
|Move Ship Forward|`W`|
|Shoot|`Space` |
|Hyperspace Travel |`X`|

## 4. Prototype Game Mechanics and Design Decisions

The purpose of this section is to outline the game mechanics and design decisions implemented in the game prototype. It serves as a reference for understanding the core gameplay elements and the rationale behind the chosen mechanics.

### Ship Movement

> _"Prepare for a wild ride through this gravity-defying adventure in the space!
> But beware, even in the vastness of the cosmos, inertia is a force to be reckoned with!"_

a. **Acceleration/Deceleration**: The spaceship can accelerate in the direction it is facing, allowing players to maneuver quickly across the screen. The spaceships retain some inertia and momentum, giving it a sense of weight and realistic movement in the game's space environment. I have dedicated special attention to this mechanic since it adds a skill-based element to the gameplay, requiring players to anticipate and adjust their movements accordingly.

b. **Ship Rotation**: Players can rotate the spaceship left or right to change the direction it is facing.

### UFOs and Asteroid Movement
> _"UFOs are well-known for their questionable driving skills making them crash into asteroids more often than any other galactic vehicle."_

The movement mechanics of UFOs and asteroids play a crucial role in the gameplay experience. Here's how their movement mechanics are designed:

a.  **Asteroids Movement**: The asteroids move in a straight path. By moving in a straightforward manner, players can strategize their shots and navigate through the asteroid-filled space more effectively.
    
b.  **UFOs Movement**: UFOs, on the other hand, to add an element of unpredictability, randomly change direction at intervals. This unexpected alteration in movement keeps players on their toes and introduces an additional layer of difficulty. It's worth noting that occasionally, UFOs may collide with asteroids due to their unpredictable direction changes.

###  Hyperspace Travel
> _"Good luck, fearless space travelers!"_

The Hyperspace Travel mechanic offers players an escape option in challenging situations adding an exciting element of unpredictability. Here are the details of this mechanic:

a. **Activation**: By pressing the hyperspace action input, the player's ship disappears from the current location and instantly reappears in a random location within the space world.

b. **Risk and Reward**: While Hyperspace Travel can provide a quick gateway from dangerous situations, its usage comes with an element of risk. There is a chance that the player's ship may reappear in close proximity to asteroids or enemies.

### Shooting

> _"Pew-pew, space cowboy! Watch out for UFOs and their bad shooting skills!
They can fire in all directions, turning asteroids into dangerous space pincushions"_

The shooting mechanic allows both the player and enemies to shoot projectiles and to engage in combat by firing at asteroids and enemies. The following features and behaviours define the shooting mechanic:

a.  **Base Projectile Capabilities**: Both the player and enemies are limited to shoot one projectile at a time. When the player initiates a shot, a projectile is fired from the spaceship "weapon" towards the direction it is facing. Due to their ship's circular geometry, UFOs have the ability to shoot projectiles in any direction. This provides an unpredictable and challenging element as UFOs can fire projectiles from various angles.
    
b.  **Continuous Firing**: Upon holding down the shoot input command, the spaceship continues to fire projectiles at a predefined fire rate. The fire rate determines the interval between consecutive shots. Releasing the shoot input command stops the firing.
    
c.  **Shooting Powerup Enhancements**: Powerups collected throughout the game can enhance the shooting mechanic. These powerups can increase the number of projectiles shot per time or enhance the fire rate, providing the player with increased firepower and combat capabilities.
    
### Wraparound Screen
> _"Who needs boundaries in space anyway?"_

To create a **seamless and boundary-free experience** in the game prototype, a wraparound mechanic is implemented. This mechanic ensures that any moving item in space, like player ship, asteroids, UFOs, and projectiles, seamlessly wraps around to the opposite side of the screen when reaching the edge. 

The **wraparound mechanic** introduces strategic opportunities for players to plan their movements and exploit the environment. They can utilize the wraparound feature to outmaneuver enemies, dodge asteroids, or gain advantageous positions.


### Asteroid Destruction
> _"Boom! Ka-pow! When projectiles meet asteroids, it's like interstellar fireworks on steroids!"_

Asteroids in the game prototype have different sizes and behave dynamically upon destruction. The following details describe the asteroid destruction mechanics:

a.  **Splitting Mechanism**: When a projectile from the player's ship or enemies collides with an asteroid, the asteroid is destroyed. Each asteroid size category (large, medium, and small) determines the resulting size and speed of the new asteroids after splitting. Larger asteroids produce medium-sized ones, while medium-sized asteroids yield smaller asteroids.
    
b.  **Size and Speed Relationship**:  The size of the asteroid directly affects its speed. Larger asteroids will move slower, while smaller asteroids will move faster. This relationship adds a challenge for players as they need to be cautious when dealing with smaller, faster-moving asteroids to avoid collisions.

### Powerups
> _"Collect them like interstellar snacks and watch your ship transform into a space-faring superhero."_

Powerups play a significant role in enhancing the player's abilities and providing temporary advantages. The following mechanics are associated with powerups:

a.  **Powerup Activation**: Powerups are spawned randomly throughout the game environment. However, only one powerup can be present at a time. When the player's ship overlaps with a powerup item, it is granted and the respective powerup effect is activated, immediately benefiting the player.
    
b.  **Limited Duration**: Each powerup has a limited duration during which its effect remains active. The duration is predetermined and set for each specific powerup.
    
c.  **Automatic Removal**: After the powerup duration expires, the effects are automatically removed from the player's ship. At this point, the player returns to their base capabilities and the powerup item is no longer active.
    
There are three different types of powerups available:

1.  **Invulnerability**: This powerup provides temporary protection to the player, shielding them from defeat caused by asteroid collisions and enemy projectiles.
    
2.  **Triple Shot**: Upon activating this powerup, the player's ship weapon receives an upgrade. It allows the ship to spawn three projectiles simultaneously, forming a conical shape. The triple shot powerup significantly increases the player's offensive capabilities.
    
3. **Double Fire Rate**: The double fire rate powerup enhances the player's weapon firing capability by doubling the base fire rate. This powerup grants the player a rapid and sustained rate of fire.
    
By incorporating random spawning of powerups with the limitation of one powerup at a time, players are encouraged to strategically collect powerups as they appear, making decisions based on their current situation and available powerup options.

### Player Respawn
  > _"Back from the void, baby! Let's give it another shot!"_
  
The player respawn mechanic gives the opportunity to bounce back after being hit by a projectile or colliding with an asteroid. When such an event occurs, the player is swiftly **respawned in the middle of the space**, granting them a fair chance to try again. A **short-duration invulnerability** powerup is **granted upon respawn**, shielding the player from subsequent collisions or enemy projectiles.

### Game Score
> _"Time to show who's the ultimate point hunter!"_

The scoring system plays an essential role in tracking the player's progress. Here's an overview of how points are awarded for each item destroied. Additionally, when the player ship collides with an asteroid or UFO, the object with which the player collides awards points as an end-match bonus. It is worth mentioning that the more enemies collide with asteroids, the more "free" points you will be granted.

Here's how points are awarded for different game elements:

---
<table>
<tr><td>

|Asteroid Size|Points|
|:--|:--|
|Large| `20`|
|Medium| `50`|
|Small|`100`|

</td><td>

|UFO Size|Points|
|:--|:--|
|Big| `200`|
|Small|`1000`|

</td></tr> </table>


### Sound Feedback
> _"The sounds of shooting and destruction are music to alien ears!"_

The prototype incorporates various sound effects to provide audio feedback for different events, enhancing the overall player experience. Here are the sound feedback events implemented in the game:

-   **Ship Destroyed**: When the player's ship is destroyed, a distinctive sound effect plays, signaling the unfortunate outcome and adding a sense of impact to the event.
-   **Asteroid or Enemy Destroyed**: When asteroids or enemies are destroyed, a satisfying explosion sound effect is triggered. This audio cue rewards the player for their successful actions and reinforces a sense of accomplishment.
-   **Shoot**: Each time the player or enemies shoot a projectile, a distinct shooting sound effect accompanies the action. This audio feedback adds immersion and responsiveness to the shooting mechanics.
-   **Power Up Equipped**: When the player collects a powerup, a special sound effect is played, indicating the successful acquisition. This audio cue enhances the sense of reward and excitement when obtaining powerups.
-   **Power Up Timed Out**: When a powerup's duration expires, a sound effect notifies the player that the effect has ended. This audio feedback helps players keep track of the powerup's duration.
-   **Hyperspace Travel**: When the player activates the Hyperspace Travel ability, a distinct sound effect accompanies the transition.

## 5. Fine-Tuning Game Values and Sounds
In order to create a well-balanced and enjoyable gameplay experience, it's important to have the **flexibility to adjust various game values** since these values directly impact the mechanics, behaviours, and overall feel of the game. You can easily **update these values in the Unity editor** to fine-tune the game.

### Change Movement Stats
To update Player Ship, Asteroids and UFOs movement behaviour follows these steps:

1.  **Open the Prefab Folder**: Navigate to `Assets` > `Prefabs`.
2.  **Locate** the component in the prefab you want to update:
	a. **Player Ship**: Double-click on `Ship`.
	b. **Large Asteroid**: Double-click on `LargeAsteroid`.
	c. **Medium Asteroid**: Double-click on `MediumAsteroid`.
	d. **Small Asteroid**:  Double-click on `SmallAsteroid`.
	e. **Large UFO**: Double-click on `LargeUfo`.
	f. **Small UFO**:Double-click on `SmallUfo`. 
3. **Select** the `Movement Stats Component` in the Prefab `Inspector`.
4. **Update** **Movement Friction**, **Acceleration**, and **Turn Rate** values according to your desired gameplay experience. 
 
### Change Weapon Stats
 To update the weapon values for Player Ship and UFOs follow these steps:
 
1.  **Open the Prefab Folder**: Navigate to `Assets` > `Prefabs`.
2.  **Locate** the component in the prefab you want to update:
	a. **Player Ship**: Double-click on `Ship`.
	b. **Large UFO**: Double-click on `LargeUfo`.
	c. **Small UFO**: Double-click on `SmallUfo`. 
3. **Select** the `Weapon Stats Component` in the Prefab `Inspector`.
4. **Update** base **Fire Rate**, **Number of Projectile to Spawn**, **Angle Between Projectiles** (when multiple), **Projectile Speed** and the **Projectile Prefab** to your convenience.

### Change Powerup Stats
 To update the powerups type and duration follow these steps:

1.  **Open the Prefab Folder**: Navigate to `Assets` > `Prefabs`.
2.  **Locate** the component in the prefab you want to update:
	a. **Pickable Powerup Double Fire Rate**: Double-click on `PickablePowerupDoubleFireRate`.
	b. **Pickable Powerup Invulnerability**: Double-click on `PickablePowerupInvulnerability`.
	c. **Pickable Powerup Triple Shot**: Double-click on `PickablePowerupTripleShot`.
3. **Select** the `Power Up Stats Component` in the Prefab `Inspector`.
4. **Update** the **type** of the powerup granted by the pickable object and the **Granted Time** of the powerup.

### Change Spawn Frequencies
 To update the spawn rates of `Asteroids`, `UFOs` and `Powerups`
 
1.  **Open the Scene**: Navigate to `Assets` > `Scenes` > `MainScene`.
2. **Select** `GameController`: in the `Hierarchy` tab.
3. **Update Frequency**: The `Inspector` tab will allow you to tweak the **Spawn Frequencies**. The higher the frequency value, the more often the item will spawn.

### Add New Prefab Variations (Asteroids, UFOs, Powerups)
To enhance the gameplay experience and keep it fresh and exciting, you can consider adding new variations of items such as Asteroids, UFOs, and Powerups. Here are the steps to add new item variations to the game:

1.  **Open the Scene**: Navigate to `Assets` > `Scenes` > `MainScene`.
2.  **Locate** `GamePrefabs`: In the `Hierarchy` tab, locate and expand the `GamePrefabs` object. 
3.  **Select** the Prefabs spawner you want to update.
4.  **Adding New Item Variations**: To add new variations of items, follow these general steps:
    a. **Asteroids**: To create new asteroid variations, you can duplicate an existing asteroid prefab (`LargeAsteroid`, `MediumAsteroid`, or `SmallAsteroid`) and modify its properties such as size, shape, or movement stats. You can also create entirely new asteroid prefabs based on your design requirements.
    b. **UFOs**: Similar to asteroids, you can duplicate the existing UFO prefab and customize its characteristics. This includes adjusting movement stats and shooting behaviour.
    c. **Powerups**: To add new powerup variations, create new powerup prefabs with new unique effects. Update the `PowerupType` enum. Finally set the duration and effects.    
5.  **Implementing New Variations**: Once you've created the new item variations, you can implement them in the game by modifying the relevant scripts or components.
 
### Update Game Sounds
You can update the game sounds for various events. Here are the steps to update the game sounds in the project:

1.  **Open the Scene**: Navigate to `Assets` > `Scenes` > `MainScene`.
2.  **Locate** `GamePrefabs`: In the `Hierarchy` tab, locate and expand the `GamePrefabs` object. 
3.  **Select** `SoundsPrefab`.
4.  **Update** the sound array to change the sfx of the linked event.

## 6. Script Folder Structure
In Unity projects, organizing scripts and files is crucial for maintaining a clean and manageable codebase. Here's an overview of the script folder structure:

```
Scripts
├── Components
│   ├── Authoring
│   ├── Commands
│   ├── Data
│   ├── Stats
│   └── Tags
├── Monobehaviours
│   └── ...
├── Systems
│   ├── Commands
│   ├── Controllers
│   └── ...
└── Utils
    └── ...
```

- **Components**: Contains all the ECS components used in the prototype.
-- **Authoring**: Contains scripts responsible for authoring components.
-- **Commands**: Holds scripts that asbract Player and AI input state.
-- **Data**: Contains scripts that represent the current entities data.
-- **Stats**: Stores scripts that allow to define game mechanics stats.
-- **Tags**: Holds tags used to identify specific entities in the ECS.
- **Monobehaviours**: Contains any MonoBehaviours that are necessary alongside the Entity Component System.
- **Systems**: Stores the scripts that handle the logic and behaviour of entities by operating on their associated components.
-- **Commands**: Holds scripts responsible for converting player and AI command into game data.
-- **Controllers**: Contains scripts that drive the commands for player and AI. It maps keyboard events and AI logic to game commands.
- **Utils**: Stores utility scripts and helper classes that can be used across different parts of the project.

### 7. Implementation Details

#### Stats and Data Components
Core feature `ECS components` have been divided into two types: `Stats` and `Data` components. This division would allow for flexible tweaking of entity behaviours and efficient memory allocation. 

- The **Stats Components** are designed to expose parameters that can be adjusted in the Unity editor, enabling different behaviours to be assigned to entities. These components typically contain data that remain constant throughout the game loop; they would be a good candidates to be turned into `ISharedComponentData` allowing a good memory chuncks management and reducing memory allocation when multiple entities share the same Stats values. 
- The **Data Components** hold the current data for each entity instance. These components contain information that vary between entities during gameplay. It stores the specific data required for entity functionalities and interactions within the game.
    
By splitting the ECS components into Stats and Data components, it is possible to achieve a better modular and flexible design. The Stats components provide a way to adjust entities behaviours easily, while the Data components hold the dynamic data specific to each entity.
Selecting which components should be implemented as Stats or Data components should be considered carefully based on their usage patterns and the need for memory optimization.

### Player and AI Commands : The Command Pattern

To abstract player and AI actions I decided to create two components: `ShootCmdComponent` and `MovementCmdComponent`. These components represent the command data that influence the behaviour of entities in the game. Let's dive into the implementation details:

a.  **`ShootCmdComponent` and `MovementCmdComponent`**: These components serve as abstracted representations of player or AI action commands. The `ShootCmdComponent` contains data related to shooting commands, while the `MovementCmdComponent` holds data for movement commands. These components allow the game systems to interpret and execute the desired actions based on the command data provided.
    
b.  **`MovementCmdSystem` and `ShootCmdSystem`**: They are responsible for processing the command data and impacting other components accordingly. The `MovementCmdSystem` combines the data from the `MovementCmdComponent` and the `MovementStatsComponent` to compute the current turn angle and acceleration for the entities. On the other hand, the `ShootCmdSystem` checks for the Shot command and communicates it to the `WeaponDataComponent`, indicating the request to shoot as soon as possible.

c. **`InputSystem`**:It simply sets the data in the `InputDataComponent` of the player entity based on the keyboard keys pressed. It is worth noticing that at this step the Input data doesn't say anything about the action that the player want to perform, it would be the `PlayerControllerSystem` then to map the `InputDataComponent` data into actions, adding modularity to the codebase and even allowing to change the input actions at runtime. 
    
d.  **`PlayerControllerSystem` and `UfoControllerSystem`**: They fill data in the `ShootCmdComponent` and `MovementCmdComponent` based on the logic implemented within them. The `PlayerControllerSystem` abstracts the player's intention by reading the currently pressed keys from the `InputDataComponent`. On the other hand, for AI entities, the `UfoControllerSystem` abstracts the intention based on custom AI logic implemented within the system itself. In this way, **the system allows AI entities to submit commands to the game just like the player does**.

This type of pattern allows to decouple the input logic from the actual execution of commands, **enabling the game to handle player and AI actions uniformly**. It promotes a more modular and flexible design, facilitating future enhancements and additions to the game's control system.

### Movement and Wrapping Systems
They play crucial roles in handling the movement of entities and enabling the Wraparound mechanic. Let's explore the implementation details of these systems:

a.  **`MovementSystem`**: It is responsible for managing the movement of entities within the game. It updates the `Translation`, `Rotation`, and `MovementDataComponent` using the data contained in the `MovementDataComponent` and `MovementStatsComponent` as inputs. This system applies movement friction and acceleration to the current entity velocity.
    
b.  **`OffScreenDetectionSystem` and `WrappingSystem`**: They work together to handle the Wraparound mechanic and ensure entities seamlessly wrap around to the opposite side of the screen when reaching the screen boundaries. The `OffScreenDetectionSystem` tracks the movement direction and camera boundaries, detecting when an entity moves off-screen. When an entity is detected to be off-screen, the `WrappingSystem` updates the entity's position, ensuring it wraps around to the opposite side of the screen.

### Collision and Destruction System
They are responsible for managing entity collisions and implementing destructible behaviour. Let's explore the implementation details of these systems:

a.  **`CollisionSystem`**: It plays a vital role in detecting entity collisions within the game. It is designed to work with entities that implement the destructible behaviour by having the `DestroyableDataComponent` and necessary physics components, such as `Physic Body` and `Physic Shape` components from the DOTS Physics package. When a collision occurs, the `CollisionSystem` identifies the entities involved and enables the `ShouldBeDestroyed` flag in the respective `DestroyableDataComponent`. However, there are certain scenarios where the `ShouldBeDestroyed` flag is not modified. One such scenario is when a collision occurs between the player and pickable powerup to prevent player destruction. Additionally, if an entity has the `IsInvulnerable` flag enabled in its `DestroyableDataComponent`, the `CollisionSystem` also doesn't mark the `ShouldBeDestroyed` flag, indicating that the entity should not be destroyed.
    
b.  **`DestroyableSystem`**: It is responsible for processing the logic to handle the destruction of entities in the game. It iterates over all entities and detects those that have the `ShouldBeDestroyed` flag enabled to be considered for destruction.

### Projectiles and Split Asteroids System
These systems are essential for the gameplay mechanics:

a.  **LifetimeSystem**: It is responsible for managing the lifespan of projectiles in the game. It uses the `LifetimeDataComponent` attached to projectiles to determine how long they should exist before timing out. If no collision occurs before the timeout, the `LifetimeSystem` accesses the `DestroyableDataComponent` and enable the `ShouldBeDestroyed` flag. This ensures that projectiles are properly cleaned up and removed from the game world after their designated lifespan expires. 

b.  **ShotProjectileSystem**: It handles the spawning of projectiles based on various parameters. It uses information from the `WeaponDataComponent`, `WeaponStatsComponent`, and `MovementDataComponent` to determine how and where to spawn projectiles. The system can spawn multiple projectiles at different fire rates, speeds, and directions, depending on the weapon statistics. By utilizing these components, the `ShotProjectileSystem` allows for customizable projectile behaviour and provides the player or AI entities with the ability to shoot projectiles with different characteristics.

c.  **SplitAsteroidsSystem**: It handles the logic for asteroid splitting when they collide with other entities or get hit by projectiles. This system plays a crucial role in implementing the "Asteroid Destruction" mechanic. It uses a `CommandBuffer` to schedule the spawning of new asteroid entities based on the game's splitting rules and the size of the original asteroid.

### Powerups System
It introduces gameplay elements that enhance the player's abilities and provide temporary advantages. Let's discuss the implementation details in two separate steps:

1. **Powerup Entity Spawning**: During the game loop pickable powerup entities are spawned randomly on the game field. These entities have collisions enabled and represent a specific powerup type and a granted time duration. The pickable powerups contain components such as `PowerupStatsComponent`, which specifies the powerup type and granted time and `DestroyableDataComponent` which allows the pickable item to be destroyed.

2. **Player Overlap and Applied Powerups**: When the player's ship overlaps with a pickable powerup entity, the player collects the powerup, triggering its effects. Upon collection, the pickable powerup entity is destroyed, and a new entity representing the ongoing powerup applied is spawned. This newly created entity contains a reference to the target entity (in our case the player's ship only, but it could target any entity) to which the powerup is applied. Additionally, it holds data that tracks the remaining time before the powerup should be removed from the target entity.

3. **PowerupSystem**: It is responsible for managing the logic of the equipped powerups. It iterates over the entities representing the applied powerups and handles the specific powerup's behaviour. The system reads powerup components such as `PowerupDataComponent` and `PowerupStatsComponent` to determine the type of powerup and its associated properties. With this information, the `PowerupSystem` controls the effects and duration of the powerup, modifying the target entity's components and thus behaviour accordingly.

### Other Game Utility Systems
In addition to the core mechanics and systems discussed earlier, the prototype includes several other scripts that contribute to the final result. Let's take a quick overview:

1. The **Random Values Generation**: Provides randomized values to entities that require them. Each entity that needs a random value has a `RandomDataComponent` attached to it. This component allows for the generation of unique random values, which can be utilized for various purposes within the game.

2.  **Game Controller**: It acts as the central hub for managing entity spawning and overall game flow. It handles custom logic to determine when and where entities, such as asteroids, UFOs, or powerups, should be spawned. The Game Controller also manages events that triggers specific actions, allowing game sounds and UI elements to react to game events.

3.  **Event Management**: To facilitate the communication and interaction between different systems, a third-party class is used to combine Unity DOTS with traditional C# events scripting. This class enable the prototype to subscribe to specific events and react accordingly.

## 8. Possible Improvements

While the current game prototype provides an entertaining experience, there are several potential areas for future enhancements and expansions. Here are some suggestions for possible improvements:

1.  **Adding New Powerups and Powerup Combinations**: Expand the powerup system by introducing new types of powerups. This could include unique abilities such as temporary speed boost, or gravity manipulators. Additionally, consider implementing a powerup combination system where player can collect multiple powerups and benefit from their combined effects. This would add depth and strategic choices to the powerup mechanics.

2.  **Powerup Stack System**: Implement a powerup stack system that allows the player to extend the duration of a powerup if it collects another powerup of the same type while the effect is still active. This would incentivize the player to actively seek out and collect powerups, creating longer-lasting advantages.

3.  **Advanced Enemies AI**: Enhance the artificial intelligence (AI) of the UFOs to make them more challenging and unpredictable opponents. Introduce more advanced behaviours would add excitement and strategic depth to the encounters with UFO enemies.

4.  **Game Loop**: Implement a game loop that grants the player a certain number of tries before encountering a game over. Each time the player is hit by a projectile or collides with an asteroid, it would lose a life.

5.  **Enhanced Visual Feedback**: Provide the player with visual feedback for all game events. This will enhance the overall experience and create a more immersive gameplay environment.

___
**Francesco Brischetto**
_Unity Game Engineer @ Bevium_
