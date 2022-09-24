using Combat;
using Core;
using Locomotion;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public float AnimationCrossFadeDuration { get; private set; } = 0.1f;
        [field: SerializeField] public float AnimatorDampTime { get; private set; } = 0.05f;
        
        public InputReader InputReader { get; private set; }
        public Mover Mover { get; private set; }
        public CameraMover CameraMover { get; private set; }
        public Animator Animator { get; private set; }
        public MeleeFighter MeleeFighter { get; private set; }

        private void Awake()
        {
            InputReader = GetComponent<InputReader>();
            Mover = GetComponent<Mover>();
            CameraMover = GetComponent<CameraMover>();
            Animator = GetComponent<Animator>();
            MeleeFighter = GetComponent<MeleeFighter>();
        }

        private void Start()
        {
            SwitchState(new PlayerLocomotionState(this));
        }
    }
}
