using UnityEngine;

namespace HorrorForestMap
{
    public class ForestMapBootstrap : MonoBehaviour
    {
        [SerializeField] private ForestMapSettings settings;

        [ContextMenu("Bootstrap Map")]
        public void Bootstrap()
        {
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<ForestMapSettings>();
            }

            GameObject mapObject = GameObject.Find("ForestMapRoot");
            if (mapObject == null)
            {
                mapObject = new GameObject("ForestMapRoot");
            }

            ForestMapBuilder builder = mapObject.GetComponent<ForestMapBuilder>();
            if (builder == null)
            {
                builder = mapObject.AddComponent<ForestMapBuilder>();
            }

            builder.settings = settings;
            builder.GenerateMap();
        }
    }
}
