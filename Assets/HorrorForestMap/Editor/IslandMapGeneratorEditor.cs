using UnityEditor;
using UnityEngine;

namespace HorrorForestMap.Editor
{
    [CustomEditor(typeof(IslandMapGenerator))]
    public class IslandMapGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            IslandMapGenerator generator = (IslandMapGenerator)target;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Map Generation", EditorStyles.boldLabel);

            if (GUILayout.Button("Generate Exact Reference Map", GUILayout.Height(40)))
            {
                generator.GenerateMap();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Clear Map", GUILayout.Height(30)))
            {
                generator.ClearMap();
                EditorUtility.SetDirty(target);
            }
        }
    }

    public static class HorrorMapMenu
    {
        [MenuItem("Horror Map/Create Island Map Generator")]
        public static void CreateIslandMapGenerator()
        {
            GameObject go = new GameObject("IslandMapGenerator");
            var generator = go.AddComponent<IslandMapGenerator>();
            Selection.activeGameObject = go;
            Debug.Log("Island Map Generator created. Add tree/rock/debris prefabs in the Inspector and click 'Generate Exact Reference Map'.");
        }

        [MenuItem("Horror Map/Open Documentation")]
        public static void OpenDocumentation()
        {
            EditorUtility.DisplayDialog("Horror Forest Map - Documentation",
                "1. Create tree, rock, and debris prefabs from your 3D models\n" +
                "2. Assign them to the IslandMapGenerator component\n" +
                "3. Click 'Generate Exact Reference Map'\n\n" +
                "The map includes:\n" +
                "- Procedural island terrain\n" +
                "- Dense and sparse forest zones\n" +
                "- Rock outcrops\n" +
                "- Forest debris\n" +
                "- Rescue base and watch tower placements\n" +
                "- Dark horror atmosphere with fog\n\n" +
                "All settings are editable in the Inspector.",
                "OK");
        }
    }
}
