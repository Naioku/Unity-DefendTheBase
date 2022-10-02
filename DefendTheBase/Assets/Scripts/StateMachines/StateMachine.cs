using UnityEngine;

namespace StateMachines
{
    public class StateMachine : MonoBehaviour
    {
        private State _currentState;

        private void Update()
        {
            _currentState?.Tick();
        }

        public void SwitchState(State state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState?.Enter();
        }
    }
}
