using InputSystem;

namespace StateMachines.Player
{
    public class PlayerStateMachine : StateMachine
    {
        private InputReader _inputReader;

        private void Awake()
        {
            _inputReader = GetComponent<InputReader>();
        }

        private void Start()
        {
            SwitchState(new PlayerTestState(this));
        }

        private void OnEnable()
        {
            _inputReader.JumpEvent += OnJump;
        }
        
        private void OnDisable()
        {
            _inputReader.JumpEvent -= OnJump;
        }

        private void OnJump()
        {
            SwitchState(new PlayerJumpState(this));
        }
    }
}
