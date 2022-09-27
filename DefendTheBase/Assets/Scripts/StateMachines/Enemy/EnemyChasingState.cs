using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyChasingState : EnemyBaseState
    {
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        private readonly List<Transform> _detectedTargets;

        public EnemyChasingState(EnemyStateMachine stateMachine, List<Transform> targets) : base(stateMachine)
        {
            _detectedTargets = targets;
        }
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 1f, StateMachine.AnimatorDampTime, Time.deltaTime);
            
            Transform closestTarget = GetClosestTarget();
            
            if (closestTarget == null)
            {
                StateMachine.SwitchState(new EnemyIdleState(StateMachine));
                return;
            }

            StateMachine.AIMover.FacePosition(closestTarget.position, deltaTime);

            if (!StateMachine.AIMover.ChaseToPosition(closestTarget.position))
            {
                StateMachine.SwitchState(new EnemyIdleState(StateMachine));
                return;
            }

            if (IsInAttackRange(closestTarget.position))
            {
                StateMachine.SwitchState(new EnemyAttackingState(StateMachine, closestTarget));
                return;
            }
        }

        private bool IsInAttackRange(Vector3 targetPosition)
        {
            return (targetPosition - StateMachine.transform.position).sqrMagnitude
                   <= Mathf.Pow(StateMachine.AttackRange, 2);
        }

        public override void Exit()
        {
            StateMachine.AIMover.StopNavMeshAgent();
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
