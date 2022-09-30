using UnityEngine;

namespace Utils.MyOffMeshLink
{
    public class OffMeshLinkAlignmentToCenter : IOffMeshLinkAlignment
    {
        public void AlignX(Transform startTransform, Transform endTransform, Vector3 centerPosition)
        {
            Vector3 startPosition = startTransform.position;
            Vector3 endPosition = endTransform.position;
            startTransform.position = new Vector3
            (
                centerPosition.x, 
                startPosition.y, 
                startPosition.z
            );
                    
            endTransform.position = new Vector3
            (
                centerPosition.x, 
                endPosition.y, 
                endPosition.z
            );
        }

        public void AlignY(Transform startTransform, Transform endTransform, Vector3 centerPosition)
        {
            Vector3 startPosition = startTransform.position;
            Vector3 endPosition = endTransform.position;
            startTransform.position = new Vector3
            (
                startPosition.x, 
                centerPosition.y, 
                startPosition.z
            );
                    
            endTransform.position = new Vector3
            (
                endPosition.x, 
                centerPosition.y, 
                endPosition.z
            );
        }

        public void AlignZ(Transform startTransform, Transform endTransform, Vector3 centerPosition)
        {
            Vector3 startPosition = startTransform.position;
            Vector3 endPosition = endTransform.position;
            startTransform.position = new Vector3
            (
                startPosition.x, 
                startPosition.y, 
                centerPosition.z
            );
                    
            endTransform.position = new Vector3
            (
                endPosition.x, 
                endPosition.y, 
                centerPosition.z
            );
        }
    }
}
