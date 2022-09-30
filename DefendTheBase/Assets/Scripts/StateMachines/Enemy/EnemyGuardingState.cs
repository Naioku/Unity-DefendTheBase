using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyGuardingState : EnemyBaseState
    {
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        public EnemyGuardingState(EnemyStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.AIMover.SwitchMovementToNavmesh();
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AISensor.TargetDetectedEvent += OnTargetDetection;
        }

        public override void Tick(float deltaTime)
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
            StateMachine.AISensor.TargetDetectedEvent -= OnTargetDetection;
        }
    }
}
