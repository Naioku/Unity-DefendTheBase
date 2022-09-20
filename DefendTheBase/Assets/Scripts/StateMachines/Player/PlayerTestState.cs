using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerTestState : PlayerBaseState
    {
        private float _timer;
    
        public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) {}
    
        public override void Enter()
        {
            Debug.Log("Entering PlayerTestState...");
            _timer = 5f;
        }

        public override void Tick(float deltaTime)
        {
            _timer -= Time.deltaTime;
            Debug.Log("_timer: " + _timer);

            if (_timer < 0f) StateMachine.SwitchState(new PlayerTestState(StateMachine));
        }

        public override void Exit()
        {
            Debug.Log("Exiting PlayerTestState...");
        }
    }
}
