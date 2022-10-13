using StateMachines.Player.Knight;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerBlockingState : PlayerBaseState
    {
        private static readonly int BlockStateHash = Animator.StringToHash("Block");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        private static readonly int RightMovementSpeedHash = Animator.StringToHash("RightMovementSpeed");

        public PlayerBlockingState(KnightStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(BlockStateHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.InputReader.BlockEvent += OnBlock;
            StateMachine.Health.TakeHitEvent += HandleBLockImpact;

            StateMachine.Health.IsVulnerable = false;
        }

        public override void Tick()
        {
            base.Tick();
            var movementDirection = CalculateMovementDirectionFromCameraPosition();
            StateMachine.PlayerMover.MoveWithBlockingStateSpeed(movementDirection);
            UpdateAnimator();
        }

        public override void Exit()
        {
            StateMachine.InputReader.BlockEvent -= OnBlock;
            StateMachine.Health.TakeHitEvent -= HandleBLockImpact;

            StateMachine.Health.IsVulnerable = true;
        }

        private void UpdateAnimator()
        {
            float movementForwardValue = StateMachine.InputReader.MovementValue.y;
            float movementRightValue = StateMachine.InputReader.MovementValue.x;

            Vector2 forwardDirection = new(0f, 1f);
            if (StateMachine.InputReader.MovementValue == Vector2.zero)
            {
                SetAnimationMovementForwardSpeed(0f);
                SetAnimationMovementRightSpeed(0f);
                return;
            }
            
            float angleDueToForwardDirection = Vector2.Angle(forwardDirection, StateMachine.InputReader.MovementValue);
            
            switch (angleDueToForwardDirection)
            {
                case <= 45f or >= 135f:
                    SetAnimationMovementForwardSpeed(Mathf.Sign(movementForwardValue));
                    SetAnimationMovementRightSpeed(0f);
                    break;
                
                default:
                    SetAnimationMovementForwardSpeed(0f);
                    SetAnimationMovementRightSpeed(Mathf.Sign(movementRightValue));
                    break;
            }
        }

        private void SetAnimationMovementForwardSpeed(float value)
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, value, StateMachine.AnimatorDampTime, Time.deltaTime);
        }

        private void SetAnimationMovementRightSpeed(float value)
        {
            StateMachine.Animator.SetFloat(RightMovementSpeedHash, value, StateMachine.AnimatorDampTime, Time.deltaTime);
        }

        private void OnBlock()
        {
            if (StateMachine.InputReader.IsBlocking) return;
            
            StateMachine.SwitchState(new PlayerLocomotionState(StateMachine));
        }
    }
}
