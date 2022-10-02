using UnityEngine;

namespace Locomotion.Player
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Transform mainCameraTransform;
        [SerializeField] private float rotationDamping = 100f;
        
        public void FaceCameraForward()
        {
            var forwardDirection = GetCameraForwardDirection();

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(forwardDirection),
                Time.deltaTime * rotationDamping);
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
