using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.AI
{
    public class AIChasingState : AIBaseState
    {
        private static readonly int LocomotionStateHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        private readonly List<Transform> _detectedTargets;
        private Vector3 _lastSeenTargetPosition;

        public AIChasingState(AIStateMachine stateMachine, List<Transform> targets) : base(stateMachine)
        {
            _detectedTargets = targets;
        }
        
        public override void Enter()
        {
            StateMachine.AIMover.SwitchMovementToNavmesh();
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionStateHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick()
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 1f, StateMachine.AnimatorDampTime, Time.deltaTime);
            
            Transform closestTarget = GetClosestTarget();

            if (closestTarget == null)
            {
                if (!StateMachine.AIMover.MoveToPosition(_lastSeenTargetPosition) ||
                    IsDestinationReached(_lastSeenTargetPosition, StateMachine.WaypointTolerance))
                {
                    StateMachine.SwitchState(new AISuspicionState(StateMachine));
                    return;
                }

                return;
            }

            _lastSeenTargetPosition = closestTarget.position;
            
            if (IsInAttackRange(_lastSeenTargetPosition))
            {
                StateMachine.SwitchState(new AICombatState(StateMachine, closestTarget));
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
