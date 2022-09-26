using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyAttackingState : EnemyBaseState
    {
        private static readonly int AttackHash = Animator.StringToHash("LeftAttack");

        public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(AttackHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            StateMachine.AIMover.OnlyApplyForces(deltaTime);

            if (HasAnimationFinished("Attack"))
            {
                StateMachine.SwitchState(new EnemyIdleState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
