using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemySuspicionState : EnemyBaseState
    {
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        private readonly Vector3 _lastSeenTargetPosition;
        private float _suspicionTimer;
        private bool _canMoveToDestination;

        public EnemySuspicionState(EnemyStateMachine stateMachine, Vector3 lastSeenTargetPosition) : base(stateMachine)
        {
            _lastSeenTargetPosition = lastSeenTargetPosition;
        }
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AISensor.TargetDetectedEvent += OnTargetDetection;
            _suspicionTimer = StateMachine.SuspicionTime;
            _canMoveToDestination = StateMachine.AIMover.MoveToPosition(_lastSeenTargetPosition);
        }

        public override void Tick(float deltaTime)
        {
            if (_canMoveToDestination &&
                !IsDestinationReached(_lastSeenTargetPosition, StateMachine.WaypointTolerance))
            {
                StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 1f, StateMachine.AnimatorDampTime, Time.deltaTime);
                return;
            }
            
            StateMachine.AIMover.StopMovement(); // shouldn't be called on every frame
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 0f, StateMachine.AnimatorDampTime, Time.deltaTime);

            _suspicionTimer -= Time.deltaTime;

            if (_suspicionTimer <= 0f)
            {
                StateMachine.SwitchState(new EnemyGuardingState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
            StateMachine.AISensor.TargetDetectedEvent -= OnTargetDetection;
        }
    }
}
