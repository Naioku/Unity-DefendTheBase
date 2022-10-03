using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
        public event Action JumpEvent;
        public event Action<MeleeAttackNames> MeleeAttackEvent;
        public event Action BlockEvent;
        public event Action CancelAttackEvent;

        public Vector2 MovementValue { get; private set; }
        public bool IsBlocking { get; private set; }

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
            
            MeleeAttackEvent?.Invoke(MeleeAttackNames.Forward);
        }

        public void OnMeleeBackwardAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            MeleeAttackEvent?.Invoke(MeleeAttackNames.Backward);
        }

        public void OnMeleeLeftAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            MeleeAttackEvent?.Invoke(MeleeAttackNames.Left);
        }

        public void OnMeleeRightAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            MeleeAttackEvent?.Invoke(MeleeAttackNames.Right);
        }

        public void OnBlockCancelAttack(InputAction.CallbackContext context)
        {
            IsBlocking = context.ReadValueAsButton();
            BlockEvent?.Invoke();
            CancelAttackEvent?.Invoke();
        }

        public void OnFreeLook(InputAction.CallbackContext context) {}
        
        
    }
}
