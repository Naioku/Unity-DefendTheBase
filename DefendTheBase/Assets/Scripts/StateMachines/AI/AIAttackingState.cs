using UnityEngine;

namespace StateMachines.AI
{
    public class AIAttackingState : AIBaseState
    {
        private static readonly int AttackHash = Animator.StringToHash("LeftAttack");

        public AIAttackingState(AIStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.AIMover.SwitchMovementToCharacterController();
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
