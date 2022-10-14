using UnityEngine;

namespace StateMachines.AI
{
    public class AIDeathState : AIBaseState
    {
        private static readonly int BlockStateHash = Animator.StringToHash("Death");

        public AIDeathState(AIStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(BlockStateHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AIMover.DisableMovement();
        }

        public override void Tick()
        {}

        public override void Exit()
        {
            StateMachine.AIMover.EnableMovement();
        }
    }
}
