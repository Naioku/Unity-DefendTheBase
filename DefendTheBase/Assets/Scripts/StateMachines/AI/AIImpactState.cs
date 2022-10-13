using UnityEngine;

namespace StateMachines.AI
{
    public class AIImpactState : AIBaseState
    {
        private static readonly int ImpactStateHash = Animator.StringToHash("Impact");

        private readonly Vector3 _hitDirection;

        public AIImpactState(AIStateMachine stateMachine, Vector3 hitDirection) : base(stateMachine)
        {
            _hitDirection = hitDirection;
        }
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(ImpactStateHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AIMover.SwitchMovementToCharacterController();
        }

        public override void Tick()
        {
            StateMachine.AIMover.ApplyForces();

            if (!HasAnimationFinished("Impact")) return;

            if (StateMachine.AIFighter.FocusOnTarget)
            {
                StateMachine.SwitchState(StateMachine.CombatState);
                return;
            }
            else
            {
                Vector3 directionFromReceiver = -_hitDirection;
                StateMachine.SwitchState(new AIRotationState(
                    StateMachine,
                    directionFromReceiver,
                    new AISuspicionState(StateMachine)
                ));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
