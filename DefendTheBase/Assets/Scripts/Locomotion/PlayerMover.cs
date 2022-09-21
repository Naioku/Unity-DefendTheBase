using UnityEngine;

namespace Locomotion
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 5f;
        
        [Header("FreeLook Camera")]
        [SerializeField] private Transform mainCameraTransform;
        [SerializeField] private float rotationDamping = 100f;

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
            _characterController.Move((movementDisplacement + _forceReceiver.ForceDisplacement) * deltaTime);
        }

        public void FaceCameraForward(float deltaTime)
        {
            var forwardDirection = GetCameraForwardDirection();

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(forwardDirection),
                deltaTime * rotationDamping);
        }

        public Vector3 GetCameraForwardDirection()
        {
            Vector3 forwardVector = mainCameraTransform.forward;
            forwardVector.y = 0f;
            forwardVector.Normalize();
            
            return forwardVector;
        }
        
        public Vector3 GetCameraRightDirection()
        {
            Vector3 rightVector = mainCameraTransform.right;
            rightVector.y = 0f;
            rightVector.Normalize();
            
            return rightVector;
        }
    }
}
