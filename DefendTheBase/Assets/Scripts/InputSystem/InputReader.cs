using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
        public event Action JumpEvent;
        
        public Vector2 MovementValue { get; private set; }
        public float IsAttackingValue { get; private set; }

        private Controls _controls;

        private void Start()
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);

            _controls.Player.Enable();
        }

        private void OnDestroy()
        {
            _controls.Player.Disable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            JumpEvent?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            IsAttackingValue = context.ReadValue<float>();
        }

        public void OnFreeLook(InputAction.CallbackContext context) {}
    }
}
