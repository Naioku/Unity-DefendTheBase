using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            Debug.Log("Entering PlayerJumpState...");
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}
