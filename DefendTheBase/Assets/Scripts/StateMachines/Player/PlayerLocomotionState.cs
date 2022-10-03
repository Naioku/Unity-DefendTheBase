using Core;
using StateMachines.Player.Knight;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerLocomotionState : PlayerBaseState
    {
        private static readonly int LocomotionStateHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        private static readonly int RightMovementSpeedHash = Animator.StringToHash("RightMovementSpeed");
        
        public PlayerLocomotionState(KnightStateMachine stateMachine) : base(stateMachine) {}
    
        public override void Enter()
        {
            StateMachine.InputReader.JumpEvent += OnJump;
            StateMachine.InputReader.MeleeAttackEvent += OnMeleeAttack;

            StateMachine.Animator.CrossFadeInFixedTime(LocomotionStateHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick()
        {
            base.Tick();
            var movementDirection = CalculateMovementDirectionFromCameraPosition();
            StateMachine.PlayerMover.MoveWithDefaultSpeed(movementDirection);
            UpdateAnimator();
        }

        public override void Exit()
        {
            StateMachine.InputReader.JumpEvent -= OnJump;
            StateMachine.InputReader.MeleeAttackEvent -= OnMeleeAttack;
        }

        private void UpdateAnimator()
        {
            float movementRightValue = StateMachine.InputReader.MovementValue.x;
            float movementForwardValue = StateMachine.InputReader.MovementValue.y;

            if (movementForwardValue == 0f)
            {
                StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 0, StateMachine.AnimatorDampTime, Time.deltaTime);
            }
            else
            {
                float value = movementForwardValue > 0f ? 1f : -1f;
                StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, value, StateMachine.AnimatorDampTime, Time.deltaTime);
            }
            
            if (movementRightValue == 0f)
            {
                StateMachine.Animator.SetFloat(RightMovementSpeedHash, 0, StateMachine.AnimatorDampTime, Time.deltaTime);
            }
            else
            {
                float value = movementRightValue > 0f ? 1f : -1f;
                StateMachine.Animator.SetFloat(RightMovementSpeedHash, value, StateMachine.AnimatorDampTime, Time.deltaTime);
            }
            
            if (StateMachine.InputReader.IsBlocking)
            {
                StateMachine.SwitchState(new PlayerBlockingState(StateMachine));
            }
        }

        private Vector3 CalculateMovementDirectionFromCameraPosition()
        {
            return StateMachine.CameraMover.GetCameraForwardDirection() * 
                   StateMachine.InputReader.MovementValue.y
                   +
                   StateMachine.CameraMover.GetCameraRightDirection() * 
                   StateMachine.InputReader.MovementValue.x;
        }

        private void OnJump()
        {
            StateMachine.SwitchState(new PlayerJumpingState(StateMachine));
        }

        private void OnMeleeAttack(MeleeAttackNames meleeAttackName)
        {
            StateMachine.SwitchState(new KnightAttackingState(StateMachine, StateMachine.MeleeFighter.GetAttack(meleeAttackName)));
        }
    }
}
