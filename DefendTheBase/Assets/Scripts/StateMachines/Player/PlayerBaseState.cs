using StateMachines.Player.Knight;

namespace StateMachines.Player
{
    public abstract class PlayerBaseState : State
    {
        protected readonly KnightStateMachine StateMachine;
        
        protected PlayerBaseState(KnightStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public override void Tick()
        {
            StateMachine.CameraMover.FaceCameraForward();
        }
        
        protected bool HasAnimationFinished(string tag)
        {
            return GetNormalizedAnimationTime(StateMachine.Animator, tag) >= 1f;
        }
    }
}
