using Locomotion.AI;
using UnityEngine;

namespace StateMachines.AI
{
    public class AIPatrollingState : AIBaseState
    {
        private static readonly int LocomotionStateHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");

        private readonly AIMover _aiMover;
        private readonly AIPatroller _aiPatroller;

        public AIPatrollingState(AIStateMachine stateMachine) : base(stateMachine)
        {
            _aiMover = StateMachine.AIMover;
            _aiPatroller = StateMachine.AIPatroller;
        }
        
        public override void Enter()
        {
            _aiMover.SwitchMovementToNavmesh();
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionStateHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AISensor.TargetDetectedEvent += OnTargetDetection;
        }

        public override void Tick()
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
