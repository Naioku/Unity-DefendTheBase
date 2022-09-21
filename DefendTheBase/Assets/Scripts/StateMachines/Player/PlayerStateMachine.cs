using InputSystem;
using Locomotion;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public float AnimationCrossFadeDuration { get; private set; } = 0.1f;
        [field: SerializeField] public float AnimatorDampTime { get; private set; } = 0.05f;
        
        public InputReader InputReader { get; private set; }
        public PlayerMover PlayerMover { get; private set; }
        public Animator Animator { get; private set; }

        private void Awake()
        {
            InputReader = GetComponent<InputReader>();
            PlayerMover = GetComponent<PlayerMover>();
            Animator = GetComponent<Animator>();
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
