using UnityEngine;

namespace Utils.MyOffMeshLink
{
    public class OffMeshLinkAlignmentToEnd : IOffMeshLinkAlignment
    {
        public void AlignX(Transform startTransform, Transform endTransform, Vector3 centerPosition)
        {
            Vector3 startPosition = startTransform.position;
            startTransform.position = new Vector3
            (
                endTransform.position.x, 
                startPosition.y, 
                startPosition.z
            );
        }

        public void AlignY(Transform startTransform, Transform endTransform, Vector3 centerPosition)
        {
            Vector3 startPosition = startTransform.position;
            startTransform.position = new Vector3
            (
                startPosition.x, 
                endTransform.position.y, 
                startPosition.z
            );
        }

        public void AlignZ(Transform startTransform, Transform endTransform, Vector3 centerPosition)
        {
            Vector3 startPosition = startTransform.position;
            startTransform.position = new Vector3
            (
                startPosition.x, 
                startPosition.y, 
                endTransform.position.z
            );
        }
    }
}
