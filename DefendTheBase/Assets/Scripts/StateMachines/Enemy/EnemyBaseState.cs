namespace StateMachines.Enemy
{
    public abstract class EnemyBaseState : State
    {
        protected readonly EnemyStateMachine StateMachine;
        
        protected EnemyBaseState(EnemyStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
        
        protected bool HasAnimationFinished(string tag)
        {
            return GetNormalizedAnimationTime(StateMachine.Animator, tag) >= 1f;
        }
    }
}
