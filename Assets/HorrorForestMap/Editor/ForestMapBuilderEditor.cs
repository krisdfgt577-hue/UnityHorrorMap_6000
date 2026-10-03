using UnityEditor;
using UnityEngine;

namespace HorrorForestMap.Editor
{
    [CustomEditor(typeof(ForestMapBuilder))]
    public class ForestMapBuilderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ForestMapBuilder builder = (ForestMapBuilder)target;

            if (GUILayout.Button("Generate Map"))
            {
                builder.GenerateMap();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Clear Map"))
            {
                builder.ClearMap();
                EditorUtility.SetDirty(target);
            }
        }
    }

    public static class HorrorMapMenu
    {
        [MenuItem("Horror Map/Create Forest Map")]
        public static void CreateForestMap()
        {
            GameObject go = new GameObject("ForestMapRoot");
            go.AddComponent<ForestMapBuilder>();
            Selection.activeGameObject = go;
        }
    }
}
