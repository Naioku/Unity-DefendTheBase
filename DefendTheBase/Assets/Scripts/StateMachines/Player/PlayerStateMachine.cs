using InputSystem;
using Locomotion;

namespace StateMachines.Player
{
    public class PlayerStateMachine : StateMachine
    {
        public InputReader InputReader { get; private set; }
        public PlayerMover PlayerMover { get; private set; }

        private void Awake()
        {
            InputReader = GetComponent<InputReader>();
            PlayerMover = GetComponent<PlayerMover>();
        }

        private void Start()
        {
            SwitchState(new PlayerLocomotionState(this));
        }

        private void OnEnable()
        {
            InputReader.JumpEvent += OnJump;
        }
        
        private void OnDisable()
        {
            InputReader.JumpEvent -= OnJump;
        }

        private void OnJump()
        {
            SwitchState(new PlayerJumpState(this));
        }
    }
}
