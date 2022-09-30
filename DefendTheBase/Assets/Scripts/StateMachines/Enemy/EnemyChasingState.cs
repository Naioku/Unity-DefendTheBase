using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyChasingState : EnemyBaseState
    {
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        private readonly List<Transform> _detectedTargets;
        private Vector3 _lastSeenTargetPosition;

        public EnemyChasingState(EnemyStateMachine stateMachine, List<Transform> targets) : base(stateMachine)
        {
            _detectedTargets = targets;
        }
        
        public override void Enter()
        {
            StateMachine.AIMover.SwitchMovementToNavmesh();
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 1f, StateMachine.AnimatorDampTime, Time.deltaTime);
            
            Transform closestTarget = GetClosestTarget();

            if (closestTarget == null)
            {
                if (!StateMachine.AIMover.MoveToPosition(_lastSeenTargetPosition))
                {
                    StateMachine.SwitchState(new EnemySuspicionState(StateMachine));
                    return;
                }
                    
                if (IsDestinationReached(_lastSeenTargetPosition, StateMachine.WaypointTolerance))
                {
                    StateMachine.SwitchState(new EnemySuspicionState(StateMachine));
                    return;
                }
                
                return;
            }

            _lastSeenTargetPosition = closestTarget.position;
            
            if (IsInAttackRange(_lastSeenTargetPosition))
            {
                StateMachine.SwitchState(new EnemyAttackingState(StateMachine, closestTarget));
                return;
            }

            if (!StateMachine.AIMover.MoveToPosition(_lastSeenTargetPosition))
            {
                StateMachine.SwitchState(new EnemySuspicionState(StateMachine));
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

        private bool IsInAttackRange(Vector3 targetPosition)
        {
            return (targetPosition - StateMachine.transform.position).sqrMagnitude
                   <= Mathf.Pow(StateMachine.AttackRange, 2);
        }
    }
}
