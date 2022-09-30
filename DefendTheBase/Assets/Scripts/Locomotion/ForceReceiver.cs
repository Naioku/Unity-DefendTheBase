using UnityEngine;

namespace Locomotion
{
    public class ForceReceiver : MonoBehaviour
    {
        public Vector3 ForceDisplacement => Vector3.up * _verticalVelocity + _impact;
        
        [SerializeField] private float impactSmoothingTime = 0.1f;
        [SerializeField] private float gravityAmplifier = 2f;
        
        private CharacterController _characterController;
        private float _verticalVelocity;
        private Vector3 _impact;
        private Vector3 _dampingVelocity;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            if (_verticalVelocity < 0f && _characterController.isGrounded)
            {
                _verticalVelocity = Physics.gravity.y * gravityAmplifier * Time.deltaTime;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * gravityAmplifier * Time.deltaTime;
            }
            
            _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, impactSmoothingTime);

            if (_impact.sqrMagnitude < 0.2f * 0.2f)
            {
                _impact = Vector3.zero;
            }
        }

        public void AddForce(Vector3 force)
        {
            _impact += force;
        }

        internal void Jump(float jumpVelocity)
        {
            _verticalVelocity += jumpVelocity;
        }
    }
}
