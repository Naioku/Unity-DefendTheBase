using Combat.AI;

namespace StateMachines.AI
{
    public class AIAttackingState : AIBaseState
    {
        private readonly AIAttack _aiAttack;
        private bool _forceAlreadyApplied;

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
            StateMachine.AIMover.ApplyForces();
            
            if (GetNormalizedAnimationTime(StateMachine.Animator, "Attack") >= _aiAttack.ForceApplicationNormalizedTime)
            {
                TryForceApplication();
            }
            
            if (HasAnimationFinished("Attack"))
            {
                StateMachine.SwitchState(new AISuspicionState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
        }

        private void TryForceApplication()
        {
            if (_forceAlreadyApplied) return;
            
            StateMachine.ForceReceiver.AddForce(
                StateMachine.transform.forward * _aiAttack.AttackerDisplacement,
                _aiAttack.AttackerImpactSmoothingTime
            );
            
            _forceAlreadyApplied = true;
        }
    }
}
