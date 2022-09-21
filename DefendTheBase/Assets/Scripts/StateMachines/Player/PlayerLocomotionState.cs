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
            StateMachine.PlayerMover.FaceCameraForward(deltaTime);
        }

        public override void Exit()
        {
            
        }
        
        private Vector3 CalculateMovementDirectionFromCameraPosition()
        {
            return StateMachine.PlayerMover.GetCameraForwardDirection() * 
                   StateMachine.InputReader.MovementValue.y
                   +
                   StateMachine.PlayerMover.GetCameraRightDirection() * 
                   StateMachine.InputReader.MovementValue.x;
        }
    }
}
