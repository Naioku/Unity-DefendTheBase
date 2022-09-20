using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerLocomotionState : PlayerBaseState
    {
        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine) {}
    
        public override void Enter()
        {
        }

        public override void Tick(float deltaTime)
        {
            var movementDirection = CalculateMovementDirectionFromCameraPosition();
            StateMachine.PlayerMover.Move(movementDirection, deltaTime);
        }

        public override void Exit()
        {
            
        }

        private Vector3 CalculateMovementDirectionFromCameraPosition()
        {
            Vector3 forwardVector = StateMachine.MainCameraTransform.forward;
            Vector3 rightVector = StateMachine.MainCameraTransform.right;

            forwardVector.y = 0f;
            rightVector.y = 0f;
            
            forwardVector.Normalize();
            rightVector.Normalize();

            return forwardVector * StateMachine.InputReader.MovementValue.y +
                   rightVector * StateMachine.InputReader.MovementValue.x;
        }
    }
}
