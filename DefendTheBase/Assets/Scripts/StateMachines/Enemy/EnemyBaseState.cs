using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Enemy
{
    public abstract class EnemyBaseState : State
    {
        protected readonly EnemyStateMachine StateMachine;
        
        protected EnemyBaseState(EnemyStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
        
        protected bool HasAnimationFinished(string tag)
        {
            return GetNormalizedAnimationTime(StateMachine.Animator, tag) >= 1f;
        }
        
        protected void OnTargetDetection(List<Transform> targets)
        {
            StateMachine.SwitchState(new EnemyChasingState(StateMachine, targets));
        }
    }
}
