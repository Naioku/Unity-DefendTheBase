using StateMachines.Player.Knight;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerBlockImpactState : PlayerBaseState
    {
        private static readonly int AttackHash = Animator.StringToHash("BlockImpact");

        public PlayerBlockImpactState(KnightStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(AttackHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.Health.OnTakeHit += HandleBLockImpact;

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
            StateMachine.Health.OnTakeHit -= HandleBLockImpact;

            StateMachine.Health.IsVulnerable = true;
        }
    }
}
