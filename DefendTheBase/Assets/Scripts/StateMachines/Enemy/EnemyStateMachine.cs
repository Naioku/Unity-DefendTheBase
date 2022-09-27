using Combat;
using Locomotion;
using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyStateMachine : StateMachine
    {
        [field: SerializeField] public float AnimationCrossFadeDuration { get; private set; } = 0.1f;
        [field: SerializeField] public float AnimatorDampTime { get; private set; } = 0.05f; 
        
        [field: Header("Attacking state")]
        [field: SerializeField] public float AttackRange { get; private set; } = 2f;
        [field: SerializeField]
        [field: Range(0f, 1f)]
        public float RotationInterpolationRatioInAttackingState { get; private set; } = 1f; 
        
        public Animator Animator { get; private set; }
        public AIMover AIMover { get; private set; }
        public AISensor AISensor { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            AIMover = GetComponent<AIMover>();
            AISensor = GetComponent<AISensor>();
        }
        
        private void Start()
        {
            SwitchState(new EnemyIdleState(this));
        }
    }
}
