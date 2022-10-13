using UnityEngine;

namespace StateMachines.AI
{
    public class AIRotationState : AIBaseState
    {
        private readonly AIBaseState _nextState;
        private readonly Vector3 _direction;
        
        private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");

        public AIRotationState(AIStateMachine stateMachine, Vector3 direction, AIBaseState nextState) : base(stateMachine)
        {
            _direction = direction;
            _nextState = nextState;
        }

        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AIMover.SwitchMovementToCharacterController();
            StateMachine.AIMover.FacePosition(_direction);
        }

        public override void Tick()
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 0f, StateMachine.AnimatorDampTime, Time.deltaTime);

            if (StateMachine.AIMover.IsRotationFinished)
            {
                StateMachine.SwitchState(_nextState);
            }
        }

        public override void Exit()
        {
        }
    }
}
