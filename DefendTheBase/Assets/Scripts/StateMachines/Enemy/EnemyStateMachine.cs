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
        [field: SerializeField] 
        public float AttackRange { get; private set; } = 2f;
        
        [field: SerializeField]
        [field: Range(0f, 1f)]
        public float RotationInterpolationRatioInAttackingState { get; private set; } = 1f;
        
        [field: Header("Suspicion state")]
        [field: SerializeField] public float SuspicionTime { get; private set; } = 2f;
        [field: SerializeField] public float WaypointTolerance { get; private set; } = 1.5f;
        
        public Vector3 GuardingPosition { get; set; }
        
        public Animator Animator { get; private set; }
        public AIMover AIMover { get; private set; }
        public AISensor AISensor { get; private set; }
        public AIPatroller AIPatroller { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            AIMover = GetComponent<AIMover>();
            AISensor = GetComponent<AISensor>();
            AIPatroller = GetComponent<AIPatroller>();
        }
        
        private void Start()
        {
            GuardingPosition = transform.position;
            SwitchToDefaultState();
        }

        public void SwitchToDefaultState()
        {
            if (AIPatroller != null)
            {
                SwitchState(new EnemyPatrollingState(this));
            }
            else
            {
                SwitchState(new EnemyGuardingState(this));
            }
        }
    }
}
