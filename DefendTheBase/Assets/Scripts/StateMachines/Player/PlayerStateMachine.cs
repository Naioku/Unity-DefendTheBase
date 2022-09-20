using InputSystem;
using Locomotion;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public Transform MainCameraTransform { get; private set; }

        public InputReader InputReader { get; private set; }
        public Animator Animator { get; private set; }
        public PlayerMover PlayerMover { get; private set; }

        private void Awake()
        {
            InputReader = GetComponent<InputReader>();
            Animator = GetComponent<Animator>();
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
