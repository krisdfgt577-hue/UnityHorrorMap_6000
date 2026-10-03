using UnityEngine;

namespace HorrorForestMap
{
    public class RescueBaseMarker : MonoBehaviour
    {
        [Header("Base zone")]
        public float safeZoneRadius = 18f;
        public Color safeZoneColor = new Color(0.22f, 0.9f, 0.4f, 0.3f);

        [Header("Visuals")]
        public Vector3 zoneSize = new Vector3(12f, 5f, 12f);

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 1f, 0.3f, 0.4f);
            Gizmos.DrawCube(transform.position + Vector3.up * 2.5f, zoneSize);

            Gizmos.color = safeZoneColor;
            Gizmos.DrawSphere(transform.position, safeZoneRadius);
        }
    }
}
