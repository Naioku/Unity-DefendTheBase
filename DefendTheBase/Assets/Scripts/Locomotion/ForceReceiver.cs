using UnityEngine;

namespace Locomotion
{
    public class ForceReceiver : MonoBehaviour
    {
        public Vector3 ForceDisplacement => Vector3.up * _verticalVelocity;
        
        private CharacterController _characterController;
        private float _verticalVelocity;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            if (_verticalVelocity < 0f && _characterController.isGrounded)
            {
                _verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }
        }
    }
}
