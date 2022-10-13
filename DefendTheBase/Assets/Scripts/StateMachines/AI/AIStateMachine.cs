using System;
using System.Collections.Generic;
using Combat;
using Combat.AI;
using Locomotion;
using Locomotion.AI;
using StateMachines.AI.Rhinbill;
using UnityEngine;

namespace StateMachines.AI
{
    public class AIStateMachine : StateMachine
    {
        [field: SerializeField] public AITypes AIType { get; private set; }
        [field: SerializeField] public float AnimationCrossFadeDuration { get; private set; } = 0.1f;
        [field: SerializeField] public float AnimatorDampTime { get; private set; } = 0.05f;
        
        [field: Header("Suspicion state")]
        [field: SerializeField] public float SuspicionTime { get; private set; } = 2f;
        [field: SerializeField] public float WaypointTolerance { get; private set; } = 1.5f;
        
        public Vector3 GuardingPosition { get; set; }
        public List<Transform> DetectedTargets { get; private set; }
        public Transform CurrentTarget { get; set; }

        public Animator Animator { get; private set; }
        public AIMover AIMover { get; private set; }
        public AISensor AISensor { get; private set; }
        public AIPatroller AIPatroller { get; private set; }
        public AIFighter AIFighter { get; private set; }
        public ForceReceiver ForceReceiver { get; private set; }
        
        public AIBaseState CombatState
        {
            get
            {
                return AIType switch
                {
                    AITypes.Rhinbill => new RhinbillCombatState(this),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        
        private Health _health;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            AIMover = GetComponent<AIMover>();
            AISensor = GetComponent<AISensor>();
            AIPatroller = GetComponent<AIPatroller>();
            AIFighter = GetComponent<AIFighter>();
            ForceReceiver = GetComponent<ForceReceiver>();
            _health = GetComponent<Health>();
        }

        private void Start()
        {
            GuardingPosition = transform.position;
            SwitchToDefaultState();
        }

        private void OnEnable()
        {
            _health.TakeDamageEventWithDirection += HandleImpact;
            AISensor.TargetDetectedEvent += HandleTargetDetection;
        }

        private void OnDisable()
        {
            _health.TakeDamageEventWithDirection -= HandleImpact;
            AISensor.TargetDetectedEvent += HandleTargetDetection;
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
        
        private void HandleImpact(Vector3 hitDirection)
        {
            SwitchState(new AIImpactState(this, hitDirection));
        }
        
        private void HandleTargetDetection(List<Transform> targets)
        {
            DetectedTargets = targets;
            
            if (!DetectedTargets.Contains(CurrentTarget))
            {
                CurrentTarget = null;
            }
        }
    }

    public enum AITypes
    {
        Rhinbill
    }
}
