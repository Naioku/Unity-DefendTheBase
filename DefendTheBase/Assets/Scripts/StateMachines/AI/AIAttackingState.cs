using UnityEngine;

namespace StateMachines.AI
{
    public class AIAttackingState : AIBaseState
    {
        private static readonly int AttackHash = Animator.StringToHash("LeftAttack");

        private readonly Transform _target;

        public AIAttackingState(AIStateMachine stateMachine, Transform target) : base(stateMachine)
        {
            _target = target;
        }
        
        public override void Enter()
        {
            StateMachine.AIMover.SwitchMovementToCharacterController();
            StateMachine.AIMover.FacePosition(_target.position, StateMachine.RotationDuration);
            StateMachine.Animator.CrossFadeInFixedTime(AttackHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            if (HasAnimationFinished("Attack"))
            {
                StateMachine.SwitchState(new AISuspicionState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
