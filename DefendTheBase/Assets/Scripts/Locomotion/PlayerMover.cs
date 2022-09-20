using UnityEngine;

namespace Locomotion
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] public float movementSpeed;

        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void Move(Vector3 direction, float deltaTime)
        {
            _characterController.Move(direction * movementSpeed * deltaTime);
        }
    }
}
