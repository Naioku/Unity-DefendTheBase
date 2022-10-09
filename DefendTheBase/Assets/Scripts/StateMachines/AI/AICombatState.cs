using Core;
using UnityEngine;

namespace StateMachines.AI
{
    public class AICombatState : AIBaseState
    {
        private static readonly int LocomotionStateHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        private readonly Transform _target;

        public AICombatState(AIStateMachine stateMachine, Transform target) : base(stateMachine)
        {
            _target = target;
        }
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionStateHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AIMover.SwitchMovementToNavmesh();
        }

        public override void Tick()
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 0f, StateMachine.AnimatorDampTime, Time.deltaTime);

            if (!IsInAttackRange(_target.position))
            {
                StateMachine.SwitchState(new AISuspicionState(StateMachine));
            }
            
            if (StateMachine.AIFighter.ReadyForNextAttack())
            {
                StateMachine.AIFighter.ResetTimer();
                StateMachine.AIFighter.StartTimer();
                Vector3 directionTowardsTarget = _target.position - StateMachine.transform.position;
                
                StateMachine.SwitchState(new AIRotationState(
                    StateMachine,
                    directionTowardsTarget,
                    new AIAttackingState(StateMachine, StateMachine.AIFighter.GetAttack(AIAttackNames.HornAttack))));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}
