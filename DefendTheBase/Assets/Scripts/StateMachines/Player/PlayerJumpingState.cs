using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerJumpingState : PlayerBaseState
    {
        private static readonly int JumpStateHash = Animator.StringToHash("Jump");
        
        public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(JumpStateHash, StateMachine.AnimationCrossFadeDuration);
            // Jump action is invoked by animation.
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            StateMachine.PlayerMover.ApplyMomentum(deltaTime);
            
            if (HasAnimationFinished("Jump") && StateMachine.PlayerMover.IsFallingDown)
            {
                StateMachine.SwitchState(new PlayerFallingDownState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
