using StateMachines.Player.Knight;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerBlockImpactState : PlayerBaseState
    {
        private static readonly int BlockImpactStateHash = Animator.StringToHash("BlockImpact");

        public PlayerBlockImpactState(KnightStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(BlockImpactStateHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.Health.TakeHitEvent += HandleBLockImpact;

            StateMachine.Health.IsVulnerable = false;
        }

        public override void Tick()
        {
            base.Tick();
            if (HasAnimationFinished("BlockImpact"))
            {
                StateMachine.SwitchState(new PlayerLocomotionState(StateMachine));
            }
        }
        
        public override void Exit()
        {
            StateMachine.Health.TakeHitEvent -= HandleBLockImpact;

            StateMachine.Health.IsVulnerable = true;
        }
    }
}
