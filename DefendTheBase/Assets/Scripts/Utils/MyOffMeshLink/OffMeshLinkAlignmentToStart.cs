using UnityEngine;

namespace Utils.MyOffMeshLink
{
    public class OffMeshLinkAlignmentToStart : IOffMeshLinkAlignment
    {
        public void AlignX(Transform startTransform, Transform endTransform, Vector3 centerPosition)
        {
            Vector3 endPosition = endTransform.position;
            endTransform.position = new Vector3
            (
                startTransform.position.x, 
                endPosition.y, 
                endPosition.z
            );
        }

        public void AlignY(Transform startTransform, Transform endTransform, Vector3 centerPosition)
        {
            Vector3 endPosition = endTransform.position;
            endTransform.position = new Vector3
            (
                endPosition.x, 
                startTransform.position.y, 
                endPosition.z
            );
        }

        public void AlignZ(Transform startTransform, Transform endTransform, Vector3 centerPosition)
        {
            Vector3 endPosition = endTransform.position;
            endTransform.position = new Vector3
            (
                endPosition.x, 
                endPosition.y, 
                startTransform.position.z
            );
        }
    }
}
