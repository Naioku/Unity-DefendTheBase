using System;
using Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
        public event Action JumpEvent;
        public event Action<AttackNames> MeleeAttackEvent;

        public Vector2 MovementValue { get; private set; }

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

        public void OnMeleeForwardAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            MeleeAttackEvent?.Invoke(AttackNames.Forward);
        }

        public void OnMeleeBackwardAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            MeleeAttackEvent?.Invoke(AttackNames.Backward);
        }

        public void OnMeleeLeftAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            MeleeAttackEvent?.Invoke(AttackNames.Left);
        }

        public void OnMeleeRightAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            MeleeAttackEvent?.Invoke(AttackNames.Right);
        }

        public void OnFreeLook(InputAction.CallbackContext context) {}
        
        
    }
}
