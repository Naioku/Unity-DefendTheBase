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
            
            if (StateMachine.InputReader.IsBlocking)
            {
                StateMachine.SwitchState(new PlayerBlockingState(StateMachine));
            }
        }

        public override void Exit()
        {
            StateMachine.InputReader.JumpEvent -= OnJump;
            StateMachine.InputReader.MeleeAttackEvent -= OnMeleeAttack;
        }

        private void UpdateAnimator()
        {
            float movementForwardValue = StateMachine.InputReader.MovementValue.y;
            float movementRightValue = StateMachine.InputReader.MovementValue.x;

            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, movementForwardValue, StateMachine.AnimatorDampTime, Time.deltaTime);
            StateMachine.Animator.SetFloat(RightMovementSpeedHash, movementRightValue, StateMachine.AnimatorDampTime, Time.deltaTime);
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
