using Combat;
using Core;

namespace StateMachines.Player
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private readonly Attack _attack;
        private bool _isComboBroken;

        public PlayerAttackingState(PlayerStateMachine stateMachine, Attack attack) : base(stateMachine)
        {
            _attack = attack;
        }
        
        public override void Enter()
        {
            StateMachine.InputReader.MeleeAttackEvent += OnMeleeAttack;

            StateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, _attack.TransitionDuration);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            if (HasAnimationFinished("Attack"))
            {
                StateMachine.SwitchState(new PlayerLocomotionState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
            StateMachine.InputReader.MeleeAttackEvent -= OnMeleeAttack;
        }

        private void OnMeleeAttack(MeleeAttackNames meleeAttackName)
        {
            if (!ReadyForNextAttack(GetNormalizedAnimationTime(StateMachine.Animator, "Attack")))
            {
                _isComboBroken = true;
                return;
            }
            
            if (_isComboBroken) return;
            
            StateMachine.SwitchState(new PlayerAttackingState(StateMachine, StateMachine.MeleeFighter.GetAttack(meleeAttackName)));
        }

        private bool ReadyForNextAttack(float normalizedAnimationTime)
        {
            return normalizedAnimationTime >= _attack.NextComboAttackNormalizedTime;
        }
    }
}
