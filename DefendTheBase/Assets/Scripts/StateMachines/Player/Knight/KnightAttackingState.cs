using Combat.Player;
using Core;

namespace StateMachines.Player.Knight
{
    public class KnightAttackingState : PlayerBaseState
    {
        private readonly MeleeAttack _meleeAttack;
        private bool _isComboBroken;

        public KnightAttackingState(KnightStateMachine stateMachine, MeleeAttack meleeAttack) : base(stateMachine)
        {
            _meleeAttack = meleeAttack;
        }
        
        public override void Enter()
        {
            StateMachine.InputReader.MeleeAttackEvent += OnMeleeAttack;
            StateMachine.InputReader.CancelAttackEvent += OnAttackCancel;

            StateMachine.Animator.CrossFadeInFixedTime(_meleeAttack.AnimationName, _meleeAttack.TransitionDuration);
        }

        public override void Tick()
        {
            base.Tick();
            if (HasAnimationFinished("Attack"))
            {
                StateMachine.SwitchState(new PlayerLocomotionState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
            StateMachine.InputReader.MeleeAttackEvent -= OnMeleeAttack;
            StateMachine.InputReader.CancelAttackEvent -= OnAttackCancel;
        }

        private void OnMeleeAttack(MeleeAttackNames meleeAttackName)
        {
            if (!ReadyForNextAttack(GetNormalizedAnimationTime(StateMachine.Animator, "Attack")))
            {
                _isComboBroken = true;
                return;
            }
            
            if (_isComboBroken) return;
            
            StateMachine.SwitchState(new KnightAttackingState(StateMachine, StateMachine.MeleeFighter.GetAttack(meleeAttackName)));
        }

        private bool ReadyForNextAttack(float normalizedAnimationTime)
        {
            return normalizedAnimationTime >= _meleeAttack.NextComboAttackNormalizedTime;
        }

        private void OnAttackCancel()
        {
            if (!CanAttackBeCancelled(GetNormalizedAnimationTime(StateMachine.Animator, "Attack"))) return;
            
            StateMachine.SwitchState(new PlayerLocomotionState(StateMachine));
        }

        private bool CanAttackBeCancelled(float normalizedAnimationTime)
        {
            return normalizedAnimationTime < _meleeAttack.CancelAttackNormalizedTime;
        }
    }
}
