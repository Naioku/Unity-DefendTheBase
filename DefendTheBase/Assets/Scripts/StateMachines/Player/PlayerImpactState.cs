using StateMachines.Player.Knight;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerImpactState : PlayerBaseState
    {
        private static readonly int FallDownStateHash = Animator.StringToHash("Impact");

        public PlayerImpactState(KnightStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(FallDownStateHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick()
        {
            base.Tick();
            StateMachine.PlayerMover.ApplyOnlyForces();
            if (HasAnimationFinished("Impact"))
            {
                StateMachine.SwitchState(new PlayerLocomotionState(StateMachine));
            }
        }

        public override void Exit()
        {
        }
    }
}
