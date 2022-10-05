using UnityEngine;

namespace StateMachines.AI
{
    public class AISuspicionState : AIBaseState
    {
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        private float _suspicionTimer;
        private bool _canMoveToDestination;

        public AISuspicionState(AIStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AISensor.TargetDetectedEvent += OnTargetDetection;
            _suspicionTimer = StateMachine.SuspicionTime;
        }

        public override void Tick()
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 0f, StateMachine.AnimatorDampTime, Time.deltaTime);

            _suspicionTimer -= Time.deltaTime;

            if (_suspicionTimer <= 0f)
            {
                StateMachine.SwitchToDefaultState();
                return;
            }
        }

        public override void Exit()
        {
            StateMachine.AISensor.TargetDetectedEvent -= OnTargetDetection;
        }
    }
}
