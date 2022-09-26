using UnityEngine;
using UnityEngine.AI;

namespace Locomotion
{
    public class AIMover : Mover
    {
        [SerializeField] private float chasingSpeed;
        
        private NavMeshAgent _navMeshAgent;

        private new void Awake()
        {
            base.Awake();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _navMeshAgent.speed = chasingSpeed;
        }

        public bool ChaseToPosition(Vector3 position)
        {
            if (!_navMeshAgent.isOnNavMesh) return false;
            if (!_navMeshAgent.SetDestination(position)) return false;
        
            return true;
        }

        public void StopNavMeshAgent()
        {
            if (!_navMeshAgent.isOnNavMesh) return;
            _navMeshAgent.ResetPath();
            _navMeshAgent.velocity = Vector3.zero;
        }

        public void FacePosition(Vector3 position)
        {
            Vector3 pointingVector = position - transform.position;
            pointingVector.y = 0f;
            
            transform.rotation = Quaternion.LookRotation(pointingVector);
        }
    }
}
