using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyIdleState : EnemyBaseState
    {
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");

        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            StateMachine.Mover.OnlyApplyForces(deltaTime);
        }

        public override void Exit()
        {
        }
    }
}
