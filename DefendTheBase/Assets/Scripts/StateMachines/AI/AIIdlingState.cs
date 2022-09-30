using UnityEngine;

namespace StateMachines.AI
{
    public class AIIdlingState : AIBaseState
    {
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");

        public AIIdlingState(AIStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AISensor.TargetDetectedEvent += OnTargetDetection;
        }

        public override void Tick(float deltaTime)
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 0f, StateMachine.AnimatorDampTime, Time.deltaTime);
            StateMachine.AIMover.ApplyForces(deltaTime);
        }

        public override void Exit()
        {
            StateMachine.AISensor.TargetDetectedEvent -= OnTargetDetection;
        }
    }
}
