using UnityEngine;

namespace StateMachines.AI
{
    public class AIGuardingState : AIBaseState
    {
        private static readonly int LocomotionStateHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        public AIGuardingState(AIStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.AIMover.SwitchMovementToNavmesh();
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionStateHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AISensor.TargetDetectedEvent += HandleTargetDetection;
        }

        public override void Tick()
        {
            if (IsDestinationReached(StateMachine.GuardingPosition, StateMachine.WaypointTolerance))
            {
                StateMachine.AIMover.StopMovement(); // shouldn't be called on every frame
                StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 0f, StateMachine.AnimatorDampTime, Time.deltaTime);
                return;
            }
            
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 1f, StateMachine.AnimatorDampTime, Time.deltaTime);
            
            if (!StateMachine.AIMover.MoveToPosition(StateMachine.GuardingPosition))
            {
                StateMachine.GuardingPosition = StateMachine.transform.position;
            }
        }

        public override void Exit()
        {
            StateMachine.AISensor.TargetDetectedEvent -= HandleTargetDetection;
        }
    }
}
