using StateMachines.Player.Knight;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerBlockingState : PlayerBaseState
    {
        private static readonly int FallDownStateHash = Animator.StringToHash("Block");

        public PlayerBlockingState(KnightStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(FallDownStateHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.InputReader.BlockEvent += OnBlock;
            StateMachine.Health.IsVulnerable = false;
        }

        public override void Exit()
        {
            StateMachine.InputReader.BlockEvent -= OnBlock;
            StateMachine.Health.IsVulnerable = true;
        }

        private void OnBlock()
        {
            if (StateMachine.InputReader.IsBlocking) return;
            
            StateMachine.SwitchState(new PlayerLocomotionState(StateMachine));
        }
    }
}
