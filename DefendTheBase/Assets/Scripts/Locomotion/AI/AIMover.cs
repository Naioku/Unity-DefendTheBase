using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Locomotion.AI
{
    public class AIMover : MonoBehaviour
    {
        public bool IsRotationFinished { get; private set; } = true;
        
        [SerializeField] private float movementSpeed = 7f;
        [SerializeField] [Tooltip("In deg/sec.")] private float rotationSpeed = 1500f;
        
        private NavMeshAgent _navMeshAgent;
        private CharacterController _characterController;
        private ForceReceiver _forceReceiver;
        private bool _isMovementDisabled;

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
        
        private void OnValidate()
        {
            ClampRotationSpeedToBeGreaterThan0();
        }

        public void DisableMovement()
        {
            _navMeshAgent.enabled = false;
            _characterController.enabled = false;
            _isMovementDisabled = true;
        }
        
        public void EnableMovement()
        {
            _isMovementDisabled = false;
        }

        public void SwitchMovementToNavmesh()
        {
            if (_isMovementDisabled) return;
            _navMeshAgent.enabled = true;
        }
        
        public void SwitchMovementToCharacterController()
        {
            if (_isMovementDisabled) return;
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

        public bool IsPositionReachable(Vector3 position)
        {
            if (!IsNavMeshAgentEnabled()) return false;
            if (!_navMeshAgent.isOnNavMesh) return false;
            
            NavMeshPath navMeshPath = new();
            _navMeshAgent.CalculatePath(position, navMeshPath);

            return navMeshPath.status == NavMeshPathStatus.PathComplete;
        }

        public void StopMovement()
        {
            if (!IsNavMeshAgentEnabled()) return;
            if (!_navMeshAgent.isOnNavMesh) return;
            
            _navMeshAgent.ResetPath();
            _navMeshAgent.velocity = Vector3.zero;
        }

        public void FacePosition(Vector3 direction)
        {
            if (!IsNavMeshAgentDisabled()) return;

            IsRotationFinished = false;
            direction.y = 0f;
           
            StartCoroutine(FacePositionCoroutine(direction, rotationSpeed));
        }

        public void ApplyForces()
        {
            if (!IsNavMeshAgentDisabled()) return;

            _characterController.Move(_forceReceiver.ForceDisplacement * Time.deltaTime);
        }
        
        private void ClampRotationSpeedToBeGreaterThan0()
        {
            rotationSpeed = Mathf.Max(rotationSpeed, float.Epsilon);
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

        private IEnumerator FacePositionCoroutine(Vector3 desiredDirection, float speedOnSec)
        {
            Quaternion startingRotation = transform.rotation;
            float rotationAngle = Vector3.Angle(transform.forward, desiredDirection);
            float speedOnFrame = speedOnSec * Time.deltaTime;
            float step = speedOnFrame / rotationAngle;
            
            for (float currentRotationFraction = 0; currentRotationFraction <= 1f; currentRotationFraction += step)
            {
                if (!IsNavMeshAgentDisabled())
                {
                    yield break;
                }
                
                transform.rotation = Quaternion.Lerp(
                    startingRotation,
                    Quaternion.LookRotation(desiredDirection, Vector3.up),
                    currentRotationFraction
                );
                
                yield return new WaitForEndOfFrame();
            }
            
            IsRotationFinished = true;
        }
    }
}
