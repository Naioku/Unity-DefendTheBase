using UnityEngine;

namespace Locomotion.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour
    {
        public bool IsFallingDown => _characterController.velocity.y < 0f;
        public bool IsGrounded => _characterController.isGrounded;
        
        [SerializeField] private float defaultSpeed = 5f;
        [SerializeField] private float blockingStateSpeed = 2.5f;
        [SerializeField] private float jumpVelocity = 5f;

        private CharacterController _characterController;
        private ForceReceiver _forceReceiver;

        protected void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _forceReceiver = GetComponent<ForceReceiver>();
        }

        public void MoveWithDefaultSpeed(Vector3 direction) => Move(direction, defaultSpeed);
        public void MoveWithBlockingStateSpeed(Vector3 direction) => Move(direction, blockingStateSpeed);

        public void ApplyOnlyForces()
        {
            UpdateVelocity(_forceReceiver.ForceDisplacement, Time.deltaTime);
        }
        
        public void ApplyMomentum()
        {
            Vector3 momentum = _characterController.velocity;
            momentum.y = 0f;
            
            UpdateVelocity(momentum + _forceReceiver.ForceDisplacement, Time.deltaTime);
        }

        private void Move(Vector3 direction, float movementSpeed)
        {
            Vector3 movementDisplacement = direction * movementSpeed;
            UpdateVelocity(movementDisplacement + _forceReceiver.ForceDisplacement, Time.deltaTime);
        }

        private void UpdateVelocity(Vector3 movement, float deltaTime)
        {
            _characterController.Move(movement * deltaTime);
        }

        // Animation event actions
        public void JumpWithDefaultVelocity()
        {
            _forceReceiver.Jump(jumpVelocity);
        }
    }
}
