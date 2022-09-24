using Combat;
using Locomotion;
using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyStateMachine : StateMachine
    {
        [field: SerializeField] public float AnimationCrossFadeDuration { get; private set; } = 0.1f;
        [field: SerializeField] public float AnimatorDampTime { get; private set; } = 0.05f; 
        
        public Animator Animator { get; private set; }
        public Mover Mover { get; private set; }
        public AISensor AISensor { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Mover = GetComponent<Mover>();
            AISensor = GetComponent<AISensor>();
        }
        
        private void Start()
        {
            SwitchState(new EnemyIdleState(this));
        }
    }
}
