using UnityEngine;

namespace Locomotion.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour
    {
        public bool IsFallingDown => _characterController.velocity.y < 0f;
        public bool IsGrounded => _characterController.isGrounded;
        
        [SerializeField] private float defaultSpeed = 5f;
        [SerializeField] private float jumpVelocity = 5f;

        private CharacterController _characterController;
        private ForceReceiver _forceReceiver;

        protected void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _forceReceiver = GetComponent<ForceReceiver>();
        }

        public void MoveWithDefaultSpeed(Vector3 direction, float deltaTime) => Move(direction, defaultSpeed, deltaTime);

        public void ApplyMomentum(float deltaTime)
        {
            Vector3 momentum = _characterController.velocity;
            momentum.y = 0f;
            
            UpdateVelocity(momentum + _forceReceiver.ForceDisplacement, deltaTime);
        }

        private void Move(Vector3 direction, float movementSpeed, float deltaTime)
        {
            Vector3 movementDisplacement = direction * movementSpeed;
            UpdateVelocity(movementDisplacement + _forceReceiver.ForceDisplacement, deltaTime);
        }

        private void UpdateVelocity(Vector3 movement, float deltaTime)
        {
            _characterController.Move(movement * deltaTime);
        }

        // Animation event actions
        public void Jump()
        {
            _forceReceiver.Jump(jumpVelocity);
        }
    }
}
