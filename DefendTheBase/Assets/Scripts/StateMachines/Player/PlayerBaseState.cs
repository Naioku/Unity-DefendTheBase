using StateMachines.Player.Knight;
using UnityEngine;

namespace StateMachines.Player
{
    public abstract class PlayerBaseState : State
    {
        protected readonly KnightStateMachine StateMachine;
        
        protected PlayerBaseState(KnightStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public override void Tick()
        {
            StateMachine.CameraMover.FaceCameraForward();
        }
        
        protected bool HasAnimationFinished(string tag)
        {
            return GetNormalizedAnimationTime(StateMachine.Animator, tag) >= 1f;
        }
        
        protected Vector3 CalculateMovementDirectionFromCameraPosition()
        {
            return StateMachine.CameraMover.GetCameraForwardDirection() * 
                   StateMachine.InputReader.MovementValue.y
                   +
                   StateMachine.CameraMover.GetCameraRightDirection() * 
                   StateMachine.InputReader.MovementValue.x;
        }
        
        protected void HandleBLockImpact()
        {
            StateMachine.SwitchState(new PlayerBlockImpactState(StateMachine));
        }
    }
}
