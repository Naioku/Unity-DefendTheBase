using Combat;
using Core;
using Locomotion.Player;
using UnityEngine;

namespace StateMachines.Player.Knight
{
    public class KnightStateMachine : StateMachine
    {
        [field: SerializeField] public float AnimationCrossFadeDuration { get; private set; } = 0.1f;
        [field: SerializeField] public float AnimatorDampTime { get; private set; } = 0.05f;
        
        public InputReader InputReader { get; private set; }
        public PlayerMover PlayerMover { get; private set; }
        public CameraMover CameraMover { get; private set; }
        public Animator Animator { get; private set; }
        public MeleeFighter MeleeFighter { get; private set; }
        public Health Health { get; private set; }

        private void Awake()
        {
            InputReader = GetComponent<InputReader>();
            PlayerMover = GetComponent<PlayerMover>();
            CameraMover = GetComponent<CameraMover>();
            Animator = GetComponent<Animator>();
            MeleeFighter = GetComponent<MeleeFighter>();
            Health = GetComponent<Health>();
        }

        private void Start()
        {
            SwitchState(new PlayerLocomotionState(this));
        }
    }
}
