using Locomotion;
using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyPatrollingState : EnemyBaseState
    {
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");

        private readonly AIMover _aiMover;
        private readonly AIPatroller _aiPatroller;

        public EnemyPatrollingState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _aiMover = StateMachine.AIMover;
            _aiPatroller = StateMachine.AIPatroller;
        }
        
        public override void Enter()
        {
            _aiMover.SwitchMovementToNavmesh();
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AISensor.TargetDetectedEvent += OnTargetDetection;
        }

        public override void Tick(float deltaTime)
        {
            if (_aiPatroller.AtWaypoint())
            {
                _aiMover.StopMovement();
                _aiPatroller.ResetTimeSinceArrivedAtWaypoint();
                _aiPatroller.ReloadWaypoint();
            }

            if (_aiPatroller.ShouldMoveToNextWaypoint())
            {
                StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 1f, StateMachine.AnimatorDampTime, Time.deltaTime);

                if (!_aiMover.MoveToPosition(_aiPatroller.GetCurrentWaypointPosition()))
                {
                    _aiPatroller.ReloadWaypoint();
                }
            }
            else
            {
                StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 0f, StateMachine.AnimatorDampTime, Time.deltaTime);
            }
            
            _aiPatroller.UpdateTimer();
        }

        public override void Exit()
        {
            StateMachine.AISensor.TargetDetectedEvent -= OnTargetDetection;
        }
    }
}
