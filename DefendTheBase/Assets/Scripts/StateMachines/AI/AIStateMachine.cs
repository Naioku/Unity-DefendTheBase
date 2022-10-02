using Combat;
using Locomotion.AI;
using UnityEngine;

namespace StateMachines.AI
{
    public class AIStateMachine : StateMachine
    {
        [field: SerializeField] public float AnimationCrossFadeDuration { get; private set; } = 0.1f;
        [field: SerializeField] public float AnimatorDampTime { get; private set; } = 0.05f;

        [field: Header("Attacking state")]
        [field: SerializeField] 
        public float AttackRange { get; private set; } = 2f;

        [field: Header("Suspicion state")]
        [field: SerializeField] public float SuspicionTime { get; private set; } = 2f;
        [field: SerializeField] public float WaypointTolerance { get; private set; } = 1.5f;
        
        public Vector3 GuardingPosition { get; set; }
        
        public Animator Animator { get; private set; }
        public AIMover AIMover { get; private set; }
        public AISensor AISensor { get; private set; }
        public AIPatroller AIPatroller { get; private set; }
        
        private Health _health;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            AIMover = GetComponent<AIMover>();
            AISensor = GetComponent<AISensor>();
            AIPatroller = GetComponent<AIPatroller>();
            _health = GetComponent<Health>();
        }
        
        private void Start()
        {
            GuardingPosition = transform.position;
            SwitchToDefaultState();
        }

        private void OnEnable()
        {
            _health.OnTakeDamage += HandleTakeDamage;
        }

        private void OnDisable()
        {
            _health.OnTakeDamage -= HandleTakeDamage;
        }
        
        public void SwitchToDefaultState()
        {
            if (AIPatroller != null)
            {
                SwitchState(new AIPatrollingState(this));
            }
            else
            {
                SwitchState(new AIGuardingState(this));
            }
        }

        private void HandleTakeDamage(Vector3 hitDirection)
        {
            SwitchState(new AIImpactState(this, hitDirection));
        }
    }
}
