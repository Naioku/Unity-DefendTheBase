using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerLocomotionState : PlayerBaseState
    {
        private static readonly int LocomotionStateHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        private static readonly int RightMovementSpeedHash = Animator.StringToHash("RightMovementSpeed");
        
        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine) {}
    
        public override void Enter()
        {
            StateMachine.InputReader.JumpEvent += OnJump;

            StateMachine.Animator.CrossFadeInFixedTime(LocomotionStateHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            var movementDirection = CalculateMovementDirectionFromCameraPosition();
            StateMachine.PlayerMover.Move(movementDirection, deltaTime);
            UpdateAnimator(deltaTime);
        }

        public override void Exit()
        {
            StateMachine.InputReader.JumpEvent -= OnJump;
        }

        private void UpdateAnimator(float deltaTime)
        {
            float movementRightValue = StateMachine.InputReader.MovementValue.x;
            float movementForwardValue = StateMachine.InputReader.MovementValue.y;

            if (movementForwardValue == 0f)
            {
                StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 0, StateMachine.AnimatorDampTime, deltaTime);
            }
            else
            {
                float value = movementForwardValue > 0f ? 1f : -1f;
                StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, value, StateMachine.AnimatorDampTime, deltaTime);
            }
            
            if (movementRightValue == 0f)
            {
                StateMachine.Animator.SetFloat(RightMovementSpeedHash, 0, StateMachine.AnimatorDampTime, deltaTime);
            }
            else
            {
                float value = movementRightValue > 0f ? 1f : -1f;
                StateMachine.Animator.SetFloat(RightMovementSpeedHash, value, StateMachine.AnimatorDampTime, deltaTime);
            }
        }

        private Vector3 CalculateMovementDirectionFromCameraPosition()
        {
            return StateMachine.PlayerMover.GetCameraForwardDirection() * 
                   StateMachine.InputReader.MovementValue.y
                   +
                   StateMachine.PlayerMover.GetCameraRightDirection() * 
                   StateMachine.InputReader.MovementValue.x;
        }
        
        private void OnJump()
        {
            StateMachine.SwitchState(new PlayerJumpingState(StateMachine));
        }
    }
}