using UnityEngine;

namespace StateMachines.AI
{
    public class AIImpactState : AIBaseState
    {
        // Temporary value. Instead of it add new impact animation and when it is finished switch state.
        private float _animationDuration = 1f;
        
        private readonly Vector3 _hitDirection;

        public AIImpactState(AIStateMachine stateMachine, Vector3 hitDirection) : base(stateMachine)
        {
            _hitDirection = hitDirection;
        }
        
        public override void Enter()
        {
            StateMachine.AIMover.SwitchMovementToCharacterController();
        }

        public override void Tick()
        {
            StateMachine.AIMover.ApplyForces();

            _animationDuration -= Time.deltaTime;
            if (_animationDuration <= 0f)
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
