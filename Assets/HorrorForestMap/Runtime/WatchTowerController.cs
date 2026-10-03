using UnityEngine;

namespace HorrorForestMap
{
    public class WatchTowerController : MonoBehaviour
    {
        [Header("Tower")]
        public float viewRadius = 70f;
        public float viewAngle = 60f;
        public float rotationSpeed = 20f;
        public Transform eyePoint;

        [Header("Targets")]
        public Transform target;
        public LayerMask targetMask;

        [Header("Visualisation")]
        public bool drawGizmos = true;

        private void Update()
        {
            if (target == null)
                return;

            if (eyePoint == null)
                eyePoint = transform;

            Vector3 dirToTarget = (target.position - eyePoint.position).normalized;
            float angle = Vector3.Angle(eyePoint.forward, dirToTarget);

            if (angle <= viewAngle * 0.5f)
            {
                float dist = Vector3.Distance(eyePoint.position, target.position);
                if (dist <= viewRadius)
                {
                    Debug.Log("Target detected by watchtower.");
                }
            }

            Quaternion targetRot = Quaternion.LookRotation(target.position - eyePoint.position, Vector3.up);
            eyePoint.rotation = Quaternion.Slerp(eyePoint.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos)
                return;

            if (eyePoint == null)
                eyePoint = transform;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(eyePoint.position, viewRadius);

            Vector3 dir1 = Quaternion.Euler(0f, -viewAngle * 0.5f, 0f) * eyePoint.forward;
            Vector3 dir2 = Quaternion.Euler(0f, viewAngle * 0.5f, 0f) * eyePoint.forward;

            Gizmos.DrawLine(eyePoint.position, eyePoint.position + dir1 * viewRadius);
            Gizmos.DrawLine(eyePoint.position, eyePoint.position + dir2 * viewRadius);
        }
    }
}
