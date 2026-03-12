# Zombie Hit Frenzy

## Project Overview
This project implements a portrait-oriented arcade zombie-hitting game inspired by Hot Slide. The player controls a drifting car in a top-down arena and earns points by hitting wandering zombies within a limited time. The objective is to score as many hits as possible before the countdown timer reaches zero.

## Prometeo Controller Adaptation

To meet the requirements of a portrait mobile game, the default Prometeo relative input system (left/right button steering) was replaced with an absolute directional swipe/drag gesture system. The goal was to replicate the arcade-like drift and responsiveness of Hot Slide.

### Absolute Directional Input
The controller reads continuous 2D screen-space touches using `Input.mousePosition` to calculate a drag delta. This 2D vector is then mapped to a normalized 3D world-space direction (`persistentDesiredDir`). A small deadzone is applied to remove micro-jitters from touch input and ensure stable directional control.

### Continuous "Hot Slide" Drive
To achieve the requested arcade drift feel, the acceleration logic was modified. When the player releases their finger, the car does not brake or reset its direction. Instead, it retains `persistentDesiredDir` and continues driving automatically in that direction until the player performs another swipe.

### Dynamic UX Feedback (Anti-Thumb Drift)
A custom UI joystick was implemented to improve mobile usability and provide visual feedback for steering direction.

- When the user taps the screen, the joystick center teleports to the touch position.
- While dragging, the joystick knob moves relative to the center and is clamped within the ring using `Vector2.ClampMagnitude`.
- When the user releases the screen, the knob locks to the outer edge of the ring based on the current `persistentDesiredDir`, giving the player a clear visual indicator of the locked steering direction.

## Gameplay Systems

### Zombie System
- Zombies wander randomly within the arena using a simple movement script.
- When hit by the player car, zombies transition into a ragdoll state.

### Ragdoll Physics
- Unity's Ragdoll Wizard was used to generate rigidbody chains for the zombie skeleton.
- Bone rigidbodies remain kinematic while the zombie is alive.
- On impact, ragdoll physics activates and an impulse force is applied to the pelvis for realistic collapse.

### Spawner System
- A centralized `ZombieSpawner` maintains the number of active zombies.
- At game start, multiple zombies spawn across the arena.
- When a zombie dies, a replacement zombie spawns after 3 seconds.
- Dead bodies remain in the scene for the duration of the round.

## Game Loop

1. Game starts and the countdown timer begins.
2. Player drives the car around the arena.
3. Hitting zombies increases the score.
4. Zombies ragdoll and remain on the ground.
5. The spawner generates replacement zombies.
6. When the timer reaches zero, the game ends.
7. A Game Over screen displays the final score.
8. The Restart button reloads the scene to begin a new round.

## HUD & UI

The game includes a minimal mobile-friendly HUD:
- Score counter
- Countdown timer
- FPS counter
- Game Over panel
- Restart button

The UI uses a Canvas Scaler configured for portrait orientation to ensure consistent layout across different screen sizes.

## Known Issues & Limitations

### Continuous Acceleration (No Reverse)
To accurately replicate the endless forward-driving arcade feel of Hot Slide, the controller is locked into continuous acceleration along `persistentDesiredDir`. As a result, the traditional Prometeo braking and reverse mechanics were intentionally deprecated. If the vehicle collides head-on with a static boundary, the player must swipe to steer and slide off the collision rather than shifting into reverse.

### Physics Step Limits
At very high drifting speeds, the car's Rigidbody may occasionally clip through a zombie without triggering the ragdoll transition. This is a known limitation of standard Unity physics step calculations on mobile devices when balancing high-speed collision detection with performance and battery efficiency.

## Summary

The project successfully implements the required gameplay systems, ragdoll physics interactions, mobile controls, and performance targets. The game loop, UI systems, and spawning mechanics are fully functional and optimized for mobile play.
