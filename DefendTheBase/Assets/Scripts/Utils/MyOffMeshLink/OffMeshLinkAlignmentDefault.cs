using UnityEngine;

namespace Utils.MyOffMeshLink
{
    public class OffMeshLinkAlignmentDefault : IOffMeshLinkAlignment
    {
        public void AlignX(Transform startTransform, Transform endTransform, Vector3 centerPosition)
        {
            PrintDefaultMessage();
        }

        public void AlignY(Transform startTransform, Transform endTransform, Vector3 centerPosition)
        {
            PrintDefaultMessage();
        }

        public void AlignZ(Transform startTransform, Transform endTransform, Vector3 centerPosition)
        {
            PrintDefaultMessage();
        }

        private void PrintDefaultMessage()
        {
            Debug.Log("Set Alignment Pivot");
        }
    }
}
