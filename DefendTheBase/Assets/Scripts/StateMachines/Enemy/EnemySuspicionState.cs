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
            _canMoveToDestination = StateMachine.AIMover.ChaseToPosition(_lastSeenTargetPosition);
        }

        public override void Tick(float deltaTime)
        {
            if (_canMoveToDestination &&
                !IsDestinationReached(_lastSeenTargetPosition, StateMachine.SuspicionWaypointTolerance))
            {
                StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 1f, StateMachine.AnimatorDampTime, Time.deltaTime);
                return;
            }
            
            StateMachine.AIMover.StopMovement();
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 0f, StateMachine.AnimatorDampTime, Time.deltaTime);

            _suspicionTimer -= Time.deltaTime;

            if (_suspicionTimer <= 0f)
            {
                StateMachine.SwitchState(new EnemyIdleState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
            StateMachine.AISensor.TargetDetectedEvent -= OnTargetDetection;
        }
        
        private bool IsDestinationReached(Vector3 destination, float displacementToleration)
        {
            float distanceToWaypointSquared = Vector3.SqrMagnitude(destination - StateMachine.transform.position);
            return distanceToWaypointSquared <= Mathf.Pow(displacementToleration, 2);
        }
    }
}
