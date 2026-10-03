using UnityEngine;

namespace HorrorForestMap
{
    public class ForestMapBuilder : MonoBehaviour
    {
        [Header("Settings")]
        public ForestMapSettings settings;

        [Header("Scene Objects")]
        [SerializeField] private GameObject mapRoot;
        [SerializeField] private GameObject forestRoot;
        [SerializeField] private GameObject decorRoot;

        public void GenerateMap()
        {
            if (settings == null)
            {
                Debug.LogError("ForestMapSettings is missing. Please assign a settings asset.");
                return;
            }

            ClearMap();

            mapRoot = new GameObject("ForestMapRoot");
            mapRoot.transform.SetParent(transform, false);

            forestRoot = new GameObject("ForestRoot");
            forestRoot.transform.SetParent(mapRoot.transform, false);

            decorRoot = new GameObject("DecorRoot");
            decorRoot.transform.SetParent(mapRoot.transform, false);

            CreateGround();
            CreateLake();
            SpawnForest();
            SpawnRocks();
            SpawnDebris();
            SpawnBaseAndTower();
            ApplyFog();
        }

        public void ClearMap()
        {
            if (mapRoot != null)
            {
                if (Application.isPlaying)
                    Destroy(mapRoot);
                else
                    DestroyImmediate(mapRoot);
            }

            mapRoot = null;
            forestRoot = null;
            decorRoot = null;
        }

        private void CreateGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.name = "Ground";
            ground.transform.SetParent(mapRoot.transform, false);
            ground.transform.localPosition = Vector3.zero;
            ground.transform.localScale = new Vector3(settings.mapSize.x, 1f, settings.mapSize.z);

            var render = ground.GetComponent<Renderer>();
            if (render != null)
            {
                Material mat = settings.terrainMaterial != null ? settings.terrainMaterial : new Material(Shader.Find("Standard"));
                mat.color = settings.terrainColor;
                render.sharedMaterial = mat;
            }
        }

        private void CreateLake()
        {
            if (settings.lakePrefab != null)
            {
                GameObject lake = Instantiate(settings.lakePrefab, new Vector3(0f, 0.2f, 10f), Quaternion.identity, decorRoot.transform);
                lake.name = "Lake";
                lake.transform.localScale = Vector3.one * 32f;
                return;
            }

            GameObject primitiveLake = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            primitiveLake.name = "Lake";
            primitiveLake.transform.SetParent(decorRoot.transform, false);
            primitiveLake.transform.localPosition = new Vector3(0f, 0.15f, 10f);
            primitiveLake.transform.localScale = new Vector3(35f, 0.5f, 35f);

            var renderer = primitiveLake.GetComponent<Renderer>();
            if (renderer != null)
            {
                var mat = new Material(Shader.Find("Standard"));
                mat.color = settings.lakeColor;
                mat.SetFloat("_Metallic", 0f);
                mat.SetFloat("_Glossiness", 0.8f);
                renderer.sharedMaterial = mat;
            }
        }

        private void SpawnForest()
        {
            if (settings.treePrefabs == null || settings.treePrefabs.Length == 0)
            {
                Debug.LogWarning("No tree prefabs assigned. Skipping forest generation.");
                return;
            }

            for (int i = 0; i < settings.treeCount; i++)
            {
                Vector3 pos = GetRandomForestPoint();
                if (pos == Vector3.zero)
                    continue;

                GameObject prefab = settings.treePrefabs[Random.Range(0, settings.treePrefabs.Length)];
                GameObject tree = Instantiate(prefab, pos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), forestRoot.transform);
                float scale = Random.Range(settings.minTreeScale, settings.maxTreeScale);
                tree.transform.localScale = new Vector3(scale, scale, scale);
                tree.name = "Tree_" + i;
            }
        }

        private Vector3 GetRandomForestPoint()
        {
            for (int attempt = 0; attempt < 80; attempt++)
            {
                float x = Random.Range(-settings.mapSize.x * 0.45f, settings.mapSize.x * 0.45f);
                float z = Random.Range(-settings.mapSize.z * 0.45f, settings.mapSize.z * 0.45f);

                float distanceFromCenter = Mathf.Sqrt(x * x + z * z);
                if (distanceFromCenter > settings.mapSize.x * 0.48f)
                    continue;

                float lakeDistance = Mathf.Sqrt((x - 0f) * (x - 0f) + (z - 10f) * (z - 10f));
                if (lakeDistance < settings.lakeRadius + 4f)
                    continue;

                return new Vector3(x, 0f, z);
            }

            return Vector3.zero;
        }

        private void SpawnRocks()
        {
            if (settings.rockPrefab == null)
                return;

            for (int i = 0; i < settings.rockCount; i++)
            {
                float x = Random.Range(-settings.mapSize.x * 0.48f, settings.mapSize.x * 0.48f);
                float z = Random.Range(-settings.mapSize.z * 0.48f, settings.mapSize.z * 0.48f);

                float distanceFromCenter = Mathf.Sqrt(x * x + z * z);
                if (distanceFromCenter > settings.mapSize.x * 0.44f)
                    continue;

                float lakeDistance = Mathf.Sqrt((x - 0f) * (x - 0f) + (z - 10f) * (z - 10f));
                if (lakeDistance < settings.lakeRadius + 8f)
                    continue;

                GameObject rock = Instantiate(settings.rockPrefab, new Vector3(x, 0.2f, z), Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), decorRoot.transform);
                rock.transform.localScale *= Random.Range(0.7f, 1.8f);
                rock.name = "Rock_" + i;
            }
        }

        private void SpawnDebris()
        {
            if (settings.debrisPrefabs == null || settings.debrisPrefabs.Length == 0)
                return;

            for (int i = 0; i < settings.debrisCount; i++)
            {
                float x = Random.Range(-settings.mapSize.x * 0.46f, settings.mapSize.x * 0.46f);
                float z = Random.Range(-settings.mapSize.z * 0.46f, settings.mapSize.z * 0.46f);
                float distanceFromCenter = Mathf.Sqrt(x * x + z * z);

                if (distanceFromCenter > settings.mapSize.x * 0.42f)
                    continue;

                GameObject prefab = settings.debrisPrefabs[Random.Range(0, settings.debrisPrefabs.Length)];
                GameObject deco = Instantiate(prefab, new Vector3(x, 0.06f, z), Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), decorRoot.transform);
                deco.transform.localScale = Vector3.one * Random.Range(0.7f, 1.5f);
                deco.name = "Debris_" + i;
            }
        }

        private void SpawnBaseAndTower()
        {
            if (settings.rescueBasePrefab != null)
            {
                GameObject baseObj = Instantiate(settings.rescueBasePrefab, settings.rescueBasePosition, Quaternion.identity, mapRoot.transform);
                baseObj.name = "RescueBase";
                var baseMarker = baseObj.GetComponent<RescueBaseMarker>();
                if (baseMarker == null)
                    baseObj.AddComponent<RescueBaseMarker>();
            }

            if (settings.watchTowerPrefab != null)
            {
                GameObject tower = Instantiate(settings.watchTowerPrefab, settings.watchTowerPosition, Quaternion.identity, mapRoot.transform);
                tower.name = "WatchTower";
                var towerController = tower.GetComponent<WatchTowerController>();
                if (towerController == null)
                    tower.AddComponent<WatchTowerController>();
            }
        }

        private void ApplyFog()
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = settings.fogColor;
            RenderSettings.fogDensity = settings.fogDensity;
        }
    }
}
