using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.AI
{
    public class AIChasingState : AIBaseState
    {
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        private readonly List<Transform> _detectedTargets;
        private Vector3 _lastSeenTargetPosition;
        // private float _timeSinceLastAttack;

        public AIChasingState(AIStateMachine stateMachine, List<Transform> targets) : base(stateMachine)
        {
            _detectedTargets = targets;
        }
        
        public override void Enter()
        {
            StateMachine.AIMover.SwitchMovementToNavmesh();
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick()
        {
            // _timeSinceLastAttack += Time.deltaTime;

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
                // // bug somewhere here
                // if (!ReadyForNextAttack())
                // {
                //     if (!StateMachine.AIMover.IsMovementStopped())
                //     {
                //         StateMachine.AIMover.StopMovement();
                //     }
                //     
                //     return;
                // }
                
                // _timeSinceLastAttack = 0f;
                Vector3 directionTowardsTarget = closestTarget.position - StateMachine.transform.position;
                StateMachine.SwitchState(new AIRotationState(
                    StateMachine,
                    directionTowardsTarget,
                    new AIAttackingState(StateMachine)));
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

        private bool IsInAttackRange(Vector3 targetPosition)
        {
            return (targetPosition - StateMachine.transform.position).sqrMagnitude
                   <= Mathf.Pow(StateMachine.AttackRange, 2);
        }

        // private bool ReadyForNextAttack()
        // {
        //     return _timeSinceLastAttack >= StateMachine.DelayBetweenAttacks;
        // }
    }
}
