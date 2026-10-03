using UnityEngine;

namespace HorrorForestMap
{
    [CreateAssetMenu(fileName = "ForestMapSettings", menuName = "Horror Map/Forest Map Settings")]
    public class ForestMapSettings : ScriptableObject
    {
        [Header("Map")]
        public Vector3 mapSize = new Vector3(350f, 20f, 350f);
        public float borderFalloff = 18f;
        public float lakeRadius = 30f;

        [Header("Ground")]
        public Material terrainMaterial;
        public Color terrainColor = new Color(0.12f, 0.18f, 0.15f, 1f);

        [Header("Forest")]
        public int treeCount = 1800;
        public GameObject[] treePrefabs;
        public float minTreeScale = 0.9f;
        public float maxTreeScale = 1.7f;

        [Header("Lake")]
        public GameObject lakePrefab;
        public Color lakeColor = new Color(0.07f, 0.42f, 0.52f, 0.9f);

        [Header("Decor")]
        public GameObject rockPrefab;
        public GameObject[] debrisPrefabs;
        public int rockCount = 90;
        public int debrisCount = 180;

        [Header("Gameplay Props")]
        public GameObject rescueBasePrefab;
        public GameObject watchTowerPrefab;
        public Vector3 rescueBasePosition = new Vector3(32f, 0.3f, -36f);
        public Vector3 watchTowerPosition = new Vector3(-44f, 0.3f, 18f);

        [Header("Atmosphere")]
        public Color fogColor = new Color(0.14f, 0.23f, 0.31f, 1f);
        [Range(0.01f, 0.2f)] public float fogDensity = 0.04f;
    }
}
