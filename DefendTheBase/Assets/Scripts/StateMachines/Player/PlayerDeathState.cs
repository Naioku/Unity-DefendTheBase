using StateMachines.Player.Knight;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerDeathState : PlayerBaseState
    {
        private static readonly int BlockStateHash = Animator.StringToHash("Death");

        public PlayerDeathState(KnightStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(BlockStateHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.PlayerMover.DisableCharacterController();
        }

        public override void Tick() {}

        public override void Exit()
        {
            StateMachine.PlayerMover.EnableCharacterController();
        }
    }
}
