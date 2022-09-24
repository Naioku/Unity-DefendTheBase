using UnityEngine;

namespace Locomotion
{
    [RequireComponent(typeof(CharacterController))]
    public class Mover : MonoBehaviour
    {
        public bool IsFallingDown => _characterController.velocity.y < 0f;
        public bool IsGrounded => _characterController.isGrounded;
        
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float jumpVelocity = 5f;

        private CharacterController _characterController;
        private ForceReceiver _forceReceiver;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _forceReceiver = GetComponent<ForceReceiver>();
        }

        public void Move(Vector3 direction, float deltaTime)
        {
            Vector3 movementDisplacement = direction * movementSpeed;
            UpdateVelocity(movementDisplacement + _forceReceiver.ForceDisplacement, deltaTime);
        }

        public void ApplyMomentum(float deltaTime)
        {
            Vector3 momentum = _characterController.velocity;
            momentum.y = 0f;
            
            UpdateVelocity(momentum + _forceReceiver.ForceDisplacement, deltaTime);
        }

        public void OnlyApplyForces(float deltaTime)
        {
            UpdateVelocity(_forceReceiver.ForceDisplacement, deltaTime);
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
