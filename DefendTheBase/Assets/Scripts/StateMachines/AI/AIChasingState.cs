using System.Collections.Generic;
using StateMachines.AI.Rhinbill;
using UnityEngine;

namespace StateMachines.AI
{
    public class AIChasingState : AIBaseState
    {
        private static readonly int LocomotionStateHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        private readonly List<Transform> _detectedTargets;
        private readonly Transform _firstDetectedTarget;
        private readonly float _targetStopRange;

        private Vector3 _lastSeenTargetPosition;

        public AIChasingState(AIStateMachine stateMachine, List<Transform> detectedTargets, float targetStopRange) : base(stateMachine)
        {
            _detectedTargets = detectedTargets;
            _firstDetectedTarget = GetClosestTarget();
            _targetStopRange = targetStopRange;
        }
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionStateHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AIMover.SwitchMovementToNavmesh();
        }

        public override void Tick()
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 1f, StateMachine.AnimatorDampTime, Time.deltaTime);

            Transform target = 
                StateMachine.FocusOnTarget ? 
                    _detectedTargets.Contains(_firstDetectedTarget) ? _firstDetectedTarget : null 
                    : GetClosestTarget();

            if (target == null)
            {
                if (!StateMachine.AIMover.MoveToPosition(_lastSeenTargetPosition) ||
                    IsDestinationReached(_lastSeenTargetPosition, StateMachine.WaypointTolerance))
                {
                    StateMachine.SwitchState(new AISuspicionState(StateMachine));
                    return;
                }

                return;
            }
            
            _lastSeenTargetPosition = target.position;
            
            if (IsInAttackRange(_lastSeenTargetPosition, _targetStopRange))
            {
                StateMachine.SwitchState(new RhinbillCombatState(StateMachine, _detectedTargets, target));
                return;
            }
            
            if (!StateMachine.AIMover.MoveToPosition(_lastSeenTargetPosition))
            {
                StateMachine.SwitchState(new AISuspicionState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
            StateMachine.AIMover.StopMovement();
        }

        private Transform GetClosestTarget()
        {
            Vector3 enemyPosition = StateMachine.transform.position;
            Transform closestTarget = null;
            float distanceToClosestTargetSquared = Mathf.Infinity;
            foreach (Transform detectedTarget in _detectedTargets)
            {
                float distanceToTargetSquared = Vector3.SqrMagnitude(detectedTarget.position - enemyPosition);
                if (distanceToTargetSquared < distanceToClosestTargetSquared)
                {
                    distanceToClosestTargetSquared = distanceToTargetSquared;
                    closestTarget = detectedTarget;
                }
            }
        
            return closestTarget;
        }
    }
}
