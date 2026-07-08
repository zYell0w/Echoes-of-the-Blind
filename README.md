# Echoes of the Blind

## Overview

Echoes of the Blind is a wave-themed Unity jam game. The project includes player movement, enemy behavior, scan-based feedback, interactable items, mission objects, dialogue resources, and menu flow. A playable build and project page are available at https://zyell0w.itch.io/echoes-of-the-blind.

## Current Status

In development.

## Unity Version

Recommended Unity version: Unity `6000.3.0f1`

## Setup

1. Install Unity `6000.3.0f1` with Unity Hub.
2. Open Unity Hub and choose **Add project from disk**.
3. Select the `Echoes-of-the-Blind` folder.
4. Open the project and allow Unity Package Manager to restore dependencies.

No additional setup is required.

## Dependencies

The project uses the following Unity packages:

| Package | Version | Purpose |
| --- | --- | --- |
| `com.unity.render-pipelines.universal` | `17.3.0` | Universal Render Pipeline support. |
| `com.unity.shadergraph` | `17.3.0` | Shader authoring for URP. |
| `com.unity.inputsystem` | `1.16.0` | Player, UI, keyboard, mouse, gamepad, touch, joystick, and XR input bindings. |
| `com.unity.ai.navigation` | `2.0.10` | NavMesh and navigation support. |
| `com.unity.ugui` | `2.0.0` | Unity UI components. |
| `com.unity.2d.sprite` | `1.0.0` | Sprite tooling. |
| `com.unity.2d.tilemap` | `1.0.0` | Tilemap tooling. |
| `com.unity.feature.characters-animation` | `1.0.0` | Character animation feature set. |
| `com.unity.ads` | `4.16.4` | Unity Ads integration package. |
| `com.unity.analytics` | `3.8.2` | Unity Analytics integration package. |
| `com.unity.purchasing` | `5.4.1` | In-app purchasing integration package. |
| `com.unity.test-framework` | `1.6.0` | Unity test tooling. |

Additional installed packages include Cinemachine `2.10.5`, Timeline `1.8.9`, Animation Rigging `1.4.0`, FBX support `5.1.4`, Burst `1.8.25`, Collections `2.6.2`, and Unity Services Core `1.18.0`.

## Tools

- Unity `6000.3.0f1`
- Unity Package Manager
- Universal Render Pipeline `17.3.0`
- Unity Input System `1.16.0`
- Unity AI Navigation `2.0.10`
- Visual Studio Editor package `2.0.25`
- Rider Editor package `3.0.38`

## Project Structure

- `Assets/Scenes` - Entry and gameplay scenes, lighting data, volume profile, and NavMesh assets.
- `Assets/Scripts/Player` - Player state, movement, camera look, input handling, item use, interaction, scan triggering, and menu toggling.
- `Assets/Scripts/Enemy` - Enemy behavior.
- `Assets/Scripts/Scan` - Scan wave logic and scan detection/listener interfaces.
- `Assets/Scripts/Missions` - Mission controller and mission object scripts for doors, windows, ammo box, TV, noise maker, and related objectives.
- `Assets/Scripts/Items` - Interactable and equipable item logic for weapons, traps, wood, blockade chair, and door bell objects.
- `Assets/Scripts/Menu` - Entry menu, settings menu, dialogue controller, and menu scanner scripts.
- `Assets/Prefabs` - Player, enemy, item, mission, and scan/wave prefabs.
- `Assets/Resources` - Fonts, images, models, dialogue data, and billing mode data.
- `Assets/Settings` - URP pipeline assets and build profile settings.

## Running the Project

The configured scene order starts with `Assets/Scenes/entry.unity`, followed by `Assets/Scenes/MainScene.unity`.

1. Open `Assets/Scenes/entry.unity`.
2. Press **Play** in the Unity Editor.

Open `Assets/Scenes/MainScene.unity` directly when working on the gameplay scene.

## Controls

| Action | Keyboard / Mouse
| --- | --- 
| Move | `WASD` 
| Look | Mouse / pointer movement
| Attack / Use weapon | Left mouse button
| Interact | `E` 
| Scan | `Space` 
| Walk | `Left Shift`
| Crouch | `C` 
| Previous item/action | `1`
| Next item/action | `2` 
| Drop item | `Q` 
| Menu | `Escape`

The player script also supports UI navigation through the Unity Input System.

## Development Notes

- The scan system drives wave feedback and can be triggered by player input.
- Player movement uses `CharacterController`, camera look, interaction raycasts, item usage, scan triggering, and simple menu toggling.
- Footstep movement can trigger smaller scan waves and walking audio.
- Interactable objects use shared item and interaction interfaces.
- Mission-related scripts are grouped around objective objects such as entry doors, windows, ammo boxes, TV, noise maker, and incense.
- Dialogue data is stored as a resource file.
- URP quality pipeline assets are organized under `Assets/Settings`.
- NavMesh assets are included with the gameplay scene.

## Build Notes

The enabled scene order is:

1. `Assets/Scenes/entry.unity`
2. `Assets/Scenes/MainScene.unity`

Build targets can be defined as the project evolves.

General build flow:

1. Open the project in Unity `6000.3.0f1`.
2. Open **File > Build Profiles** or **File > Build Settings**.
3. Confirm the enabled scenes are in the intended order.
4. Select a target platform.
5. Build the project.

## Credits / License

This project is licensed under the GNU General Public License, Version 3.
Asset, plugin, and package-specific license notes can be added here when needed.
