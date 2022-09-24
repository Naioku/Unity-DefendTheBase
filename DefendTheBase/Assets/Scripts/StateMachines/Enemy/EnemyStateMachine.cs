using Locomotion;
using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyStateMachine : StateMachine
    {
        [field: SerializeField] public float AnimationCrossFadeDuration { get; private set; } = 0.1f;

        public Animator Animator { get; private set; }
        public Mover Mover { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Mover = GetComponent<Mover>();
        }
        
        private void Start()
        {
            SwitchState(new EnemyIdleState(this));
        }
    }
}
