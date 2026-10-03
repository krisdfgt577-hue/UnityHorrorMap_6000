# Unity Horror Forest Map

Editable horror forest map for Unity 6000.4.1f1, inspired by the provided reference images.

## What is included
- Procedural forest map generator
- Editable map configuration via ScriptableObject
- Rescue base marker
- Watch tower logic
- Editor menu for quick setup
- Fog / atmosphere setup
- Ready to be imported into a Unity project

## Folder structure
- `Packages/manifest.json`
- `ProjectSettings/ProjectVersion.txt`
- `Assets/HorrorForestMap/Runtime/`
- `Assets/HorrorForestMap/Editor/`

## Usage in Unity
1. Open this folder as a Unity project.
2. Create an empty GameObject in a scene.
3. Add `ForestMapBuilder` component.
4. Assign a `ForestMapSettings` asset or create one via `Assets -> Create -> Horror Map -> Forest Map Settings`.
5. Press `Generate Map` in the Inspector.
6. Or use the menu: `Horror Map -> Create Forest Map`.

## Notes
This is an editable-map prototype with a dark forest island, a lake, an observation tower, and a rescue base. Replace the placeholder prefabs with actual tree, rock, and building models to match your final artistic direction.
