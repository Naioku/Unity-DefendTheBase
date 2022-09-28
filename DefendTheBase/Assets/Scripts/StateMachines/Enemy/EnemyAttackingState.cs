using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyAttackingState : EnemyBaseState
    {
        private static readonly int AttackHash = Animator.StringToHash("LeftAttack");

        private readonly Transform _target;

        public EnemyAttackingState(EnemyStateMachine stateMachine, Transform target) : base(stateMachine)
        {
            _target = target;
        }
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(AttackHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AIMover.FacePosition(_target.position, StateMachine.RotationInterpolationRatioInAttackingState);
        }

        public override void Tick(float deltaTime)
        {
            StateMachine.AIMover.OnlyApplyForces(deltaTime);

            if (HasAnimationFinished("Attack"))
            {
                StateMachine.SwitchState(new EnemySuspicionState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
