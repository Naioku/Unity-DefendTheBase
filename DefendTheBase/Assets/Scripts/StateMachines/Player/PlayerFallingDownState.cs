using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerFallingDownState : PlayerBaseState
    {
        private static readonly int FallDownStateHash = Animator.StringToHash("FallDown");
        
        public PlayerFallingDownState(PlayerStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(FallDownStateHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            StateMachine.Mover.ApplyMomentum(deltaTime);

            if (StateMachine.Mover.IsGrounded)
            {
                StateMachine.SwitchState(new PlayerGroundingState(StateMachine));
            }
        }

        public override void Exit()
        {
        }
    }
}
