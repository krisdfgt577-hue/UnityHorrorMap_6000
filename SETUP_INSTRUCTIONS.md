# How to Clone and Setup Unity Horror Forest Map

## Prerequisites
- Git installed on your computer ([git-scm.com](https://git-scm.com/))
- Unity 6000.4.1f1 installed
- A GitHub account (already have one)

---

## Step 1: Clone the Repository

### On Windows (PowerShell or Command Prompt):
```bash
git clone https://github.com/krisdfgt577-hue/UnityHorrorMap_6000.git
```

### On Mac/Linux (Terminal):
```bash
git clone https://github.com/krisdfgt577-hue/UnityHorrorMap_6000.git
```

This creates a folder `UnityHorrorMap_6000` with all project files.

---

## Step 2: Open in Unity

1. Open **Unity Hub**
2. Click **"Add"** → Select the `UnityHorrorMap_6000` folder
3. Select **Unity 6000.4.1f1** as the version
4. Click to open the project

---

## Step 3: Setup the Map

### Option A: Quick Setup (Recommended)
1. In Unity menu, go to **Horror Map** → **Create Island Map Generator**
2. A new GameObject `IslandMapGenerator` will appear in the Hierarchy
3. Select it and look at the Inspector

### Option B: Manual Setup
1. In the Hierarchy, right-click → **Create Empty**
2. Name it `IslandMapGenerator`
3. With it selected, go to Inspector → **Add Component**
4. Search for and add `IslandMapGenerator`

---

## Step 4: Assign Prefabs

The map generator needs prefabs for trees, rocks, and debris:

### Create Tree Prefabs:
1. Import or create 3D tree models (FBX, OBJ, or 3D objects)
2. Drag one into the Hierarchy to create a prefab
3. Right-click in Project → **Create** → **Prefab** → **Prefab (New)** (or drag from Hierarchy to a Prefabs folder)
4. Repeat for 2-3 different tree variations

### Assign to Generator:
1. Select the `IslandMapGenerator` object
2. In Inspector, find **"Tree Prefabs"** field
3. Set the array size (e.g., 3)
4. Drag your tree prefabs into the slots

### Do the same for:
- **Rock Prefabs** (assign 1-2 rock models)
- **Debris Prefabs** (assign 1-2 small objects like branches, logs, etc.)

### Optional Structures:
- **Rescue Base Prefab** (building model)
- **Watch Tower Prefab** (tower model)

---

## Step 5: Generate the Map

1. Select the `IslandMapGenerator` in Hierarchy
2. In Inspector, click **"Generate Exact Reference Map"** button
3. Wait for generation (this may take 30 seconds to 2 minutes depending on tree count)
4. The map will appear in the Hierarchy under `HorrorIslandMap`

---

## Step 6: Customization

All settings are editable in the Inspector:

### Island Settings:
- **islandRadius**: Size of the island (default: 175)
- **islandRoughness**: Terrain roughness (0-1)
- **meshResolution**: Mesh detail level

### Forest Settings:
- **treeCountDense**: Trees in center area (default: 2200)
- **treeCountSparse**: Trees at edges (default: 600)
- **minTreeScale / maxTreeScale**: Tree size variation

### Atmosphere:
- **fogColor**: Color of fog
- **fogDensity**: How thick the fog is (0.02-0.15)

### Structure Positions:
- **rescueBasePosition**: Where the rescue base spawns
- **watchTowerPosition**: Where the watch tower spawns

---

## Step 7: Add a Camera

To see the map:
1. Create a new **Camera** (Right-click → **Camera**)
2. Set position to something like (0, 80, -100) to look at the island from above
3. Press **Play** to test

---

## Troubleshooting

### "No tree prefabs assigned" warning
- Make sure you have created or imported tree models
- Drag them into the Hierarchy first
- Then create Prefabs from them
- Assign to the `IslandMapGenerator` component

### Map generation is slow
- Reduce `treeCountDense` and `treeCountSparse`
- Increase `meshResolution` for faster but less detailed terrain

### Map looks too dark
- Adjust **fogDensity** in the Inspector (lower = less fog)
- Adjust **RenderSettings.ambientLight** in the scene

### Structures don't appear
- Create prefabs for `rescueBasePrefab` and `watchTowerPrefab`
- Assign them in the Inspector
- Re-generate the map

---

## Repository Details

- **Repository URL**: https://github.com/krisdfgt577-hue/UnityHorrorMap_6000
- **Unity Version**: 6000.4.1f1
- **Namespace**: `HorrorForestMap`
- **Main Script**: `IslandMapGenerator.cs`
- **Editor Tools**: `Horror Map` menu in Unity

---

## What the Map Includes

✅ Procedural island terrain with height variation  
✅ Dense forest in the center  
✅ Sparse forest at the edges  
✅ Rock outcrops scattered across the island  
✅ Forest debris (fallen logs, branches, etc.)  
✅ Water surrounding the island  
✅ Dark horror atmosphere with fog  
✅ Marked positions for rescue base and watch tower  
✅ Fully editable in the Inspector  
✅ All assets organized in a clean hierarchy  

---

## Next Steps

1. Replace placeholder prefabs with your own high-quality 3D models
2. Add lighting and post-processing for more horror atmosphere
3. Add player character and gameplay mechanics
4. Create rescue base and tower interiors
5. Add sound design and particle effects

---

**Happy development!** 🎮
