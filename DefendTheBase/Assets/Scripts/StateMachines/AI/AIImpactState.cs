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

        public override void Tick(float deltaTime)
        {
            StateMachine.AIMover.ApplyForces(deltaTime);

            _animationDuration -= deltaTime;
            if (_animationDuration <= 0f)
            {
                StateMachine.SwitchState(new AIRotationToAttackerState(StateMachine, _hitDirection));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
