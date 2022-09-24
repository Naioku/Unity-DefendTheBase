using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyChasingState : EnemyBaseState
    {
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        private readonly List<Transform> _targets;

        public EnemyChasingState(EnemyStateMachine stateMachine, List<Transform> targets) : base(stateMachine)
        {
            _targets = targets;
        }
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 1f, StateMachine.AnimatorDampTime, Time.deltaTime);
            
            if (_targets.Count == 0)
            {
                StateMachine.SwitchState(new EnemyIdleState(StateMachine));
            }
        }

        public override void Exit()
        {
        }
    }
}
