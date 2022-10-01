using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Locomotion.AI
{
    public class AIMover : MonoBehaviour
    {
        public bool IsFallingDown => _characterController.velocity.y < 0f;
        public bool IsGrounded => _characterController.isGrounded;
        public bool IsRotationFinished { get; private set; } = true;
        
        [SerializeField] private float movementSpeed = 7f;
        
        private NavMeshAgent _navMeshAgent;
        private CharacterController _characterController;
        private ForceReceiver _forceReceiver;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _characterController = GetComponent<CharacterController>();
            _forceReceiver = GetComponent<ForceReceiver>();
        }

        private void Start()
        {
            _navMeshAgent.speed = movementSpeed;
        }

        public void SwitchMovementToNavmesh()
        {
            _navMeshAgent.enabled = true;
        }
        
        public void SwitchMovementToCharacterController()
        {
            _navMeshAgent.enabled = false;
        }

        public bool MoveToPosition(Vector3 position)
        {
            if (!IsNavMeshAgentEnabled()) return false;
            if (!_navMeshAgent.isOnNavMesh) return false;
            if (!_navMeshAgent.SetDestination(position)) return false;
            if (!IsPathBuilt(_navMeshAgent.path)) return false;
        
            return true;
        }

        public void StopMovement()
        {
            if (!IsNavMeshAgentEnabled()) return;
            if (!_navMeshAgent.isOnNavMesh) return;
            
            _navMeshAgent.ResetPath();
            _navMeshAgent.velocity = Vector3.zero;
        }

        public void FacePosition(Vector3 position, float duration)
        {
            if (!IsNavMeshAgentDisabled()) return;

            IsRotationFinished = false;
            Vector3 pointingVector = position - transform.position;
            pointingVector.y = 0f;
            StartCoroutine(FacePositionCoroutine(pointingVector, duration));
        }

        public void ApplyForces(float deltaTime)
        {
            if (!IsNavMeshAgentDisabled()) return;

            _characterController.Move(_forceReceiver.ForceDisplacement * deltaTime);
        }

        private bool IsPathBuilt(NavMeshPath path)
        {
            return path.status == NavMeshPathStatus.PathComplete;
        }

        private bool IsNavMeshAgentDisabled()
        {
            if (!_navMeshAgent.enabled) return true;
            
            print("AIMover: Nav mesh agent IS NOT disabled.");
            return false;
        }

        private bool IsNavMeshAgentEnabled()
        {
            if (_navMeshAgent.enabled) return true;
            
            print("AIMover: Nav mesh agent IS disabled.");
            return false;
        }

        private IEnumerator FacePositionCoroutine(Vector3 pointingVector, float duration)
        {
            for (float time = 0; time < duration; time += Time.deltaTime)
            {
                if (!IsNavMeshAgentDisabled())
                {
                    yield break;
                }
                
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(pointingVector),
                    time/duration
                );

                yield return new WaitForEndOfFrame();
            }

            IsRotationFinished = true;
        }
    }
}
