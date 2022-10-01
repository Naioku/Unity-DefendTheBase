using UnityEngine;

namespace StateMachines.AI
{
    public class AIRotationToAttackerState : AIBaseState
    {
        private readonly Vector3 _hitDirectionFromReceiver;

        public AIRotationToAttackerState(AIStateMachine stateMachine, Vector3 hitDirection) : base(stateMachine)
        {
            _hitDirectionFromReceiver = -hitDirection;
        }
        
        public override void Enter()
        {
            StateMachine.AIMover.FacePosition(_hitDirectionFromReceiver, StateMachine.RotationDuration);
        }

        public override void Tick(float deltaTime)
        {
            if (StateMachine.AIMover.IsRotationFinished)
            {
                StateMachine.SwitchState(new AISuspicionState(StateMachine));
            }
        }

        public override void Exit()
        {
        }
    }
}
