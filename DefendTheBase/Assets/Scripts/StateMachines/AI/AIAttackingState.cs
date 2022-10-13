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
            StateMachine.Animator.CrossFadeInFixedTime(_aiAttack.AnimationName, _aiAttack.TransitionDuration);
            StateMachine.AIMover.SwitchMovementToCharacterController();
        }

        public override void Tick()
        {
            StateMachine.AIMover.ApplyForces();
            
            if (GetNormalizedAnimationTime(StateMachine.Animator, "Attack") >= _aiAttack.DisplacementApplicationNormalizedTime)
            {
                TryForceApplication();
            }
            
            if (HasAnimationFinished("Attack"))
            {
                StateMachine.SwitchState(StateMachine.CombatState);
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
                _aiAttack.AttackerDisplacementSmoothingTime
            );
            
            _forceAlreadyApplied = true;
        }
    }
}
