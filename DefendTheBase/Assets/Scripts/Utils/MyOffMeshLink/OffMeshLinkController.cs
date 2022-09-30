using UnityEngine;
using UnityEngine.AI;

namespace Utils.MyOffMeshLink
{
    [ExecuteAlways]
    [RequireComponent(typeof(OffMeshLink))]
    public class OffMeshLinkController : MonoBehaviour
    {
        [SerializeField] private Color lineColor;

        [Header("Start point")]
        [SerializeField] private Color startPointColor;
        [SerializeField] private float startPointRadius;

        [Header("End point")]
        [SerializeField] private Color endPointColor;
        [SerializeField] private float endPointRadius;


        [Header("Alignment")] [SerializeField]
        private AlignmentPivot alignmentPivot;

        private OffMeshLink _offMeshLink;

        private IOffMeshLinkAlignment AlignmentManager
        {
            get
            {
                IOffMeshLinkAlignment result = alignmentPivot switch
                {
                    AlignmentPivot.ToStartPoint => new OffMeshLinkAlignmentToStart(),
                    AlignmentPivot.ToEndPoint => new OffMeshLinkAlignmentToEnd(),
                    AlignmentPivot.ToCenterPoint => new OffMeshLinkAlignmentToCenter(),
                    _ => new OffMeshLinkAlignmentDefault()
                };

                return result;
            }
        }
        
        private void OnDrawGizmos()
        {
            UpdateLinkPointsAppearance();
            UpdateConnectingLine();
        }

        private void Awake()
        {
            _offMeshLink = GetComponent<OffMeshLink>();
        }

        public void RecenterControllerPosition()
        {
            if (!TryGetStartEndTransforms(out var startTransform, out var endTransform)) return;
            
            var startPosition = startTransform.position;
            var endPosition = endTransform.position;

            var centerPosition = GetCenterPositionBetweenPoints(startPosition, endPosition);

            transform.position = centerPosition;
            
            _offMeshLink.startTransform.position = startPosition;
            _offMeshLink.endTransform.position = endPosition;
        }

        public void AlignStartEndPositionInX()
        {
            if (!TryGetStartEndTransforms(out var startTransform, out var endTransform)) return;

            var centerPosition = GetCenterPositionBetweenPoints(startTransform.position, endTransform.position);
            
            AlignmentManager.AlignX(startTransform, endTransform, centerPosition);
            RecenterControllerPosition();
        }

        public void AlignStartEndPositionInY()
        {
            if (!TryGetStartEndTransforms(out var startTransform, out var endTransform)) return;

            var centerPosition = GetCenterPositionBetweenPoints(startTransform.position, endTransform.position);
            
            AlignmentManager.AlignY(startTransform, endTransform, centerPosition);
            RecenterControllerPosition();
        }

        public void AlignStartEndPositionInZ()
        {
            if (!TryGetStartEndTransforms(out var startTransform, out var endTransform)) return;
            
            var centerPosition = GetCenterPositionBetweenPoints(startTransform.position, endTransform.position);
            
            AlignmentManager.AlignZ(startTransform, endTransform, centerPosition);
            RecenterControllerPosition();
        }

        private void UpdateLinkPointsAppearance()
        {
            if (!TryGetStartEndTransforms(out var startTransform, out var endTransform)) return;

            Gizmos.color = startPointColor;
            Gizmos.DrawSphere(startTransform.position, startPointRadius);

            Gizmos.color = endPointColor;
            Gizmos.DrawSphere(endTransform.position, endPointRadius);
        }

        private void UpdateConnectingLine()
        {
            if (!TryGetStartEndTransforms(out var startTransform, out var endTransform)) return;

            Gizmos.color = lineColor;
            Gizmos.DrawLine(startTransform.position, endTransform.position);
        }

        private static Vector3 GetCenterPositionBetweenPoints(Vector3 startPosition, Vector3 endPosition)
        {
            return startPosition + (endPosition - startPosition) / 2;
        }
        
        private bool TryGetStartEndTransforms(out Transform startTransform, out Transform endTransform)
        {
            startTransform = _offMeshLink.startTransform;
            endTransform = _offMeshLink.endTransform;
            
            return (startTransform != null && endTransform != null);
        }
    }
    
    internal enum AlignmentPivot
    {
        ToStartPoint,
        ToEndPoint,
        ToCenterPoint
    }
}
