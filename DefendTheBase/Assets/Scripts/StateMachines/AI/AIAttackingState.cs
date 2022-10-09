using Combat.AI;

namespace StateMachines.AI
{
    public class AIAttackingState : AIBaseState
    {
        private readonly AIAttack _aiAttack;

        public AIAttackingState(AIStateMachine stateMachine, AIAttack aiAttack) : base(stateMachine)
        {
            _aiAttack = aiAttack;
        }
        
        public override void Enter()
        {
            StateMachine.AIMover.SwitchMovementToCharacterController();
            StateMachine.Animator.CrossFadeInFixedTime(_aiAttack.AnimationName, _aiAttack.TransitionDuration);
        }

        public override void Tick()
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
