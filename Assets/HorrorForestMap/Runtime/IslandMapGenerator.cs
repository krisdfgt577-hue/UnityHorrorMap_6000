using UnityEngine;
using System.Collections.Generic;

namespace HorrorForestMap
{
    public class IslandMapGenerator : MonoBehaviour
    {
        [Header("Island Shape")]
        public float islandRadius = 175f;
        public float islandRoughness = 0.4f;
        public AnimationCurve islandFalloff = AnimationCurve.EaseInOut(0, 1, 1, 0);
        public Vector3 islandCenter = Vector3.zero;

        [Header("Terrain")]
        public Material terrainMaterial;
        public Color terrainColorDark = new Color(0.08f, 0.12f, 0.10f, 1f);
        public Color terrainColorGreen = new Color(0.15f, 0.24f, 0.18f, 1f);
        public float meshResolution = 2f;

        [Header("Water")]
        public Material waterMaterial;
        public Color waterColor = new Color(0.05f, 0.35f, 0.45f, 0.95f);
        public float waterLevel = -0.5f;

        [Header("Forest Density")]
        public int treeCountDense = 2200;
        public int treeCountSparse = 600;
        public GameObject[] treePrefabs;
        public float minTreeScale = 1.1f;
        public float maxTreeScale = 2.2f;
        public float treePlacementRadius = 165f;

        [Header("Rock Outcrops")]
        public GameObject[] rockPrefabs;
        public int rockCount = 140;
        public float rockMinScale = 1f;
        public float rockMaxScale = 2.5f;

        [Header("Forest Debris")]
        public GameObject[] debrisPrefabs;
        public int debrisCount = 250;

        [Header("Structures")]
        public GameObject rescueBasePrefab;
        public GameObject watchTowerPrefab;
        public Vector3 rescueBasePosition = new Vector3(60f, 0f, -80f);
        public Vector3 watchTowerPosition = new Vector3(-90f, 0f, 50f);

        [Header("Atmosphere")]
        public Color fogColor = new Color(0.10f, 0.18f, 0.25f, 1f);
        [Range(0.02f, 0.15f)] public float fogDensity = 0.08f;
        public Color skyColor = new Color(0.12f, 0.20f, 0.28f, 1f);

        [Header("Internal")]
        [SerializeField] private GameObject mapRoot;
        [SerializeField] private GameObject terrainObject;
        [SerializeField] private GameObject forestRoot;
        [SerializeField] private GameObject rocksRoot;
        [SerializeField] private GameObject debrisRoot;
        [SerializeField] private GameObject structuresRoot;
        [SerializeField] private GameObject waterObject;

        private List<Vector3> usedPositions = new List<Vector3>();

        [ContextMenu("Generate Exact Reference Map")]
        public void GenerateMap()
        {
            ClearMap();
            CreateMapHierarchy();
            CreateTerrainMesh();
            CreateWater();
            CreateDenseForest();
            CreateSparseForest();
            CreateRockOutcrops();
            CreateForestDebris();
            CreateStructures();
            ApplyAtmosphere();
            Debug.Log("Horror forest map generated!");
        }

        [ContextMenu("Clear Map")]
        public void ClearMap()
        {
            if (mapRoot != null)
            {
                if (Application.isPlaying)
                    Destroy(mapRoot);
                else
                    DestroyImmediate(mapRoot);
            }
            usedPositions.Clear();
        }

        private void CreateMapHierarchy()
        {
            mapRoot = new GameObject("HorrorIslandMap");
            mapRoot.transform.SetParent(transform);

            terrainObject = new GameObject("Terrain");
            terrainObject.transform.SetParent(mapRoot.transform);

            waterObject = new GameObject("Water");
            waterObject.transform.SetParent(mapRoot.transform);

            forestRoot = new GameObject("Forest");
            forestRoot.transform.SetParent(mapRoot.transform);

            rocksRoot = new GameObject("Rocks");
            rocksRoot.transform.SetParent(mapRoot.transform);

            debrisRoot = new GameObject("Debris");
            debrisRoot.transform.SetParent(mapRoot.transform);

            structuresRoot = new GameObject("Structures");
            structuresRoot.transform.SetParent(mapRoot.transform);
        }

        private void CreateTerrainMesh()
        {
            GameObject terrain = new GameObject("IslandTerrain");
            terrain.transform.SetParent(terrainObject.transform);

            Mesh mesh = GenerateIslandMesh();
            var meshFilter = terrain.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            var meshCollider = terrain.AddComponent<MeshCollider>();
            meshCollider.mesh = mesh;

            var renderer = terrain.AddComponent<MeshRenderer>();
            Material mat = terrainMaterial != null ? terrainMaterial : new Material(Shader.Find("Standard"));
            mat.color = terrainColorDark;
            renderer.material = mat;
        }

        private Mesh GenerateIslandMesh()
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            int gridSize = (int)(islandRadius * 2f / meshResolution);
            float halfSize = islandRadius;

            // Create vertex grid
            for (int z = 0; z <= gridSize; z++)
            {
                for (int x = 0; x <= gridSize; x++)
                {
                    float posX = -halfSize + x * meshResolution;
                    float posZ = -halfSize + z * meshResolution;

                    float distFromCenter = Vector2.Distance(new Vector2(posX, posZ), new Vector2(islandCenter.x, islandCenter.z));
                    float normalizedDist = Mathf.Clamp01(distFromCenter / islandRadius);

                    // Island falloff curve
                    float falloff = islandFalloff.Evaluate(normalizedDist);
                    float height = Mathf.Lerp(3f, 0f, normalizedDist);
                    height += Mathf.PerlinNoise(posX * 0.05f, posZ * 0.05f) * 2f * falloff;

                    vertices.Add(new Vector3(posX, height * falloff, posZ));
                }
            }

            // Create triangles
            for (int z = 0; z < gridSize; z++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    int a = z * (gridSize + 1) + x;
                    int b = a + 1;
                    int c = a + gridSize + 1;
                    int d = c + 1;

                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(b);

                    triangles.Add(b);
                    triangles.Add(c);
                    triangles.Add(d);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        private void CreateWater()
        {
            GameObject water = GameObject.CreatePrimitive(PrimitiveType.Plane);
            water.name = "WaterPlane";
            water.transform.SetParent(waterObject.transform);
            water.transform.localPosition = new Vector3(0, waterLevel, 0);
            water.transform.localScale = new Vector3(450f, 1f, 450f);

            var renderer = water.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = waterMaterial != null ? waterMaterial : new Material(Shader.Find("Standard"));
                mat.color = waterColor;
                mat.SetFloat("_Metallic", 0.2f);
                mat.SetFloat("_Smoothness", 0.9f);
                renderer.material = mat;
            }
        }

        private void CreateDenseForest()
        {
            GameObject denseForest = new GameObject("DenseForest");
            denseForest.transform.SetParent(forestRoot.transform);

            if (treePrefabs == null || treePrefabs.Length == 0)
            {
                Debug.LogWarning("No tree prefabs assigned.");
                return;
            }

            int placed = 0;
            int attempts = 0;
            int maxAttempts = treeCountDense * 3;

            while (placed < treeCountDense && attempts < maxAttempts)
            {
                Vector3 pos = GetRandomDenseForestPoint();
                if (pos != Vector3.zero && !IsPositionTooClose(pos, 8f))
                {
                    GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                    GameObject tree = Instantiate(prefab, pos, Quaternion.Euler(0, Random.Range(0f, 360f), 0), denseForest.transform);
                    float scale = Random.Range(minTreeScale, maxTreeScale);
                    tree.transform.localScale = new Vector3(scale, scale, scale);
                    usedPositions.Add(pos);
                    placed++;
                }
                attempts++;
            }
        }

        private void CreateSparseForest()
        {
            GameObject sparseForest = new GameObject("SparseForest");
            sparseForest.transform.SetParent(forestRoot.transform);

            if (treePrefabs == null || treePrefabs.Length == 0)
                return;

            int placed = 0;
            int attempts = 0;
            int maxAttempts = treeCountSparse * 3;

            while (placed < treeCountSparse && attempts < maxAttempts)
            {
                Vector3 pos = GetRandomSparseForestPoint();
                if (pos != Vector3.zero && !IsPositionTooClose(pos, 12f))
                {
                    GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                    GameObject tree = Instantiate(prefab, pos, Quaternion.Euler(0, Random.Range(0f, 360f), 0), sparseForest.transform);
                    float scale = Random.Range(minTreeScale * 0.8f, maxTreeScale * 1.2f);
                    tree.transform.localScale = new Vector3(scale, scale, scale);
                    usedPositions.Add(pos);
                    placed++;
                }
                attempts++;
            }
        }

        private Vector3 GetRandomDenseForestPoint()
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float radius = Random.Range(30f, treePlacementRadius * 0.7f);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            // Lake avoidance
            float lakeX = 0f, lakeZ = 20f;
            float lakeRadius = 55f;
            float distToLake = Vector2.Distance(new Vector2(x, z), new Vector2(lakeX, lakeZ));
            if (distToLake < lakeRadius)
                return Vector3.zero;

            return new Vector3(x, 0f, z);
        }

        private Vector3 GetRandomSparseForestPoint()
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float radius = Random.Range(treePlacementRadius * 0.6f, treePlacementRadius);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            return new Vector3(x, 0f, z);
        }

        private bool IsPositionTooClose(Vector3 pos, float minDistance)
        {
            foreach (Vector3 usedPos in usedPositions)
            {
                if (Vector3.Distance(pos, usedPos) < minDistance)
                    return true;
            }
            return false;
        }

        private void CreateRockOutcrops()
        {
            if (rockPrefabs == null || rockPrefabs.Length == 0)
                return;

            for (int i = 0; i < rockCount; i++)
            {
                Vector3 pos = GetRandomRockPosition();
                if (pos != Vector3.zero && !IsPositionTooClose(pos, 15f))
                {
                    GameObject prefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
                    GameObject rock = Instantiate(prefab, pos, Quaternion.Euler(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f)), rocksRoot.transform);
                    float scale = Random.Range(rockMinScale, rockMaxScale);
                    rock.transform.localScale = Vector3.one * scale;
                    usedPositions.Add(pos);
                }
            }
        }

        private Vector3 GetRandomRockPosition()
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float radius = Random.Range(80f, treePlacementRadius);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            return new Vector3(x, 0.5f, z);
        }

        private void CreateForestDebris()
        {
            if (debrisPrefabs == null || debrisPrefabs.Length == 0)
                return;

            for (int i = 0; i < debrisCount; i++)
            {
                float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                float radius = Random.Range(20f, treePlacementRadius * 0.9f);
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                GameObject prefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Length)];
                GameObject debris = Instantiate(prefab, new Vector3(x, 0.1f, z), Quaternion.Euler(0, Random.Range(0, 360f), 0), debrisRoot.transform);
                debris.transform.localScale = Vector3.one * Random.Range(0.8f, 1.6f);
            }
        }

        private void CreateStructures()
        {
            if (rescueBasePrefab != null)
            {
                GameObject baseObj = Instantiate(rescueBasePrefab, rescueBasePosition, Quaternion.identity, structuresRoot.transform);
                baseObj.name = "RescueBase";
                var marker = baseObj.GetComponent<RescueBaseMarker>();
                if (marker == null)
                    baseObj.AddComponent<RescueBaseMarker>();
            }

            if (watchTowerPrefab != null)
            {
                GameObject tower = Instantiate(watchTowerPrefab, watchTowerPosition, Quaternion.identity, structuresRoot.transform);
                tower.name = "WatchTower";
                var controller = tower.GetComponent<WatchTowerController>();
                if (controller == null)
                    tower.AddComponent<WatchTowerController>();
            }
        }

        private void ApplyAtmosphere()
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.ambientLight = new Color(0.4f, 0.4f, 0.5f);
        }
    }
}
