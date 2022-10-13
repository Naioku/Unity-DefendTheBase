using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.AI
{
    public abstract class AIBaseState : State
    {
        protected readonly AIStateMachine StateMachine;
        
        protected AIBaseState(AIStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
        
        protected bool HasAnimationFinished(string tag)
        {
            return GetNormalizedAnimationTime(StateMachine.Animator, tag) >= 1f;
        }
        
        protected void HandleTargetDetection(List<Transform> targets)
        {
            StateMachine.AIMover.SwitchMovementToNavmesh();
            Transform closestTarget = GetClosestReachableTarget();
            if (closestTarget == null) return;
            
            StateMachine.CurrentTarget = closestTarget;

            StateMachine.SwitchState(new AIChasingState(StateMachine, StateMachine.AIFighter.MaxAttackRange));
        }

        protected Transform GetClosestReachableTarget()
        {
            Vector3 aiPosition = StateMachine.transform.position;
            Transform closestTarget = null;
            float distanceToClosestTargetSquared = Mathf.Infinity;
            foreach (Transform detectedTarget in StateMachine.DetectedTargets)
            {
                if(!StateMachine.AIMover.IsPositionReachable(detectedTarget.position)) continue;
                
                float distanceToTargetSquared = Vector3.SqrMagnitude(detectedTarget.position - aiPosition);
                if (distanceToTargetSquared < distanceToClosestTargetSquared)
                {
                    distanceToClosestTargetSquared = distanceToTargetSquared;
                    closestTarget = detectedTarget;
                }
            }
        
            return closestTarget;
        }

        protected bool IsDestinationReached(Vector3 destination, float displacementToleration)
        {
            float distanceToWaypointSquared = Vector3.SqrMagnitude(destination - StateMachine.transform.position);
            return distanceToWaypointSquared <= Mathf.Pow(displacementToleration, 2);
        }
    }
}
