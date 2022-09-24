using System.Collections.Generic;
using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyIdleState : EnemyBaseState
    {
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");

        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AISensor.TargetDetectedEvent += OnTargetDetection;
        }

        public override void Tick(float deltaTime)
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 0f, StateMachine.AnimatorDampTime, Time.deltaTime);
            StateMachine.Mover.OnlyApplyForces(deltaTime);
        }

        public override void Exit()
        {
            StateMachine.AISensor.TargetDetectedEvent -= OnTargetDetection;
        }

        private void OnTargetDetection(List<Transform> targets)
        {
            StateMachine.SwitchState(new EnemyChasingState(StateMachine, targets));
        }
    }
}
