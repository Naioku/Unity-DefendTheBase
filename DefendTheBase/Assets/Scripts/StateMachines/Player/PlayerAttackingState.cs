using Combat;

namespace StateMachines.Player
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private readonly Attack _attack;

        public PlayerAttackingState(PlayerStateMachine stateMachine, Attack attack) : base(stateMachine)
        {
            _attack = attack;
        }
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, _attack.TransitionDuration);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            if (GetNormalizedAnimationTime(StateMachine.Animator, "Attack") >= 1f)
            {
                StateMachine.SwitchState(new PlayerLocomotionState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
