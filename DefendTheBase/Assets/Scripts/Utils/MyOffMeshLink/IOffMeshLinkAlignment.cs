using UnityEngine;

namespace Utils.MyOffMeshLink
{
    public interface IOffMeshLinkAlignment
    {
        public void AlignX(Transform startTransform, Transform endTransform, Vector3 centerPosition);
        public void AlignY(Transform startTransform, Transform endTransform, Vector3 centerPosition);
        public void AlignZ(Transform startTransform, Transform endTransform, Vector3 centerPosition);
    }
}
