using UnityEngine;

namespace Utils.MyOffMeshLink
{
    [ExecuteAlways]
    public class OffMeshLinkPosition : MonoBehaviour
    {
        public Color GizmoColor { get; set; } = Color.cyan;
        public float GizmoRadius { get; set; } = 0.2f;

        private void OnDrawGizmos()
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawSphere(transform.position, GizmoRadius);
        }
    }
}
