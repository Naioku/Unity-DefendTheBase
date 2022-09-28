using UnityEngine;
using UnityEngine.AI;

namespace Locomotion
{
    public class AIMover : Mover
    {
        [SerializeField] private float movingSpeed = 7f;
        [SerializeField] [Range(0f, 1f)] private float rotationInterpolationRatio = 0.1f;
        
        private NavMeshAgent _navMeshAgent;

        private new void Awake()
        {
            base.Awake();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _navMeshAgent.speed = movingSpeed;
            _navMeshAgent.updateRotation = false;
        }

        public bool MoveToPosition(Vector3 position)
        {
            if (!_navMeshAgent.isOnNavMesh) return false;
            if (!_navMeshAgent.SetDestination(position)) return false;
        
            return true;
        }
        
        public void StopMovement()
        {
            if (!_navMeshAgent.isOnNavMesh) return;
            _navMeshAgent.ResetPath();
            _navMeshAgent.velocity = Vector3.zero;
        }

        public void FacePosition(Vector3 position)
        {
            FacePosition(position, rotationInterpolationRatio);
        }
        
        public void FacePosition(Vector3 position, float rotationInterpolationRatio)
        {
            Vector3 pointingVector = position - transform.position;
            pointingVector.y = 0f;
            
            transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(pointingVector),
                    rotationInterpolationRatio
            );
        }
    }
}
