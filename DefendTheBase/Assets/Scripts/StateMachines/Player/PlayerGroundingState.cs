using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerGroundingState : PlayerBaseState
    {
        private static readonly int GroundStateHash = Animator.StringToHash("Ground");
        
        public PlayerGroundingState(PlayerStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(GroundStateHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            
            if (HasAnimationFinished("Ground"))
            {
                StateMachine.SwitchState(new PlayerLocomotionState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
