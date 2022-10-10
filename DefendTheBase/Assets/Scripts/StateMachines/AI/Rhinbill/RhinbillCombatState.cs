using System.Collections.Generic;
using Combat.AI;
using Core;
using UnityEngine;

namespace StateMachines.AI.Rhinbill
{
    public class RhinbillCombatState : AIBaseState
    {
        private static readonly int LocomotionStateHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        private readonly List<Transform> _detectedTargets;
        private readonly Transform _targetReceived;

        // It must not be null for now. Change it, because some Rhinbills won't have all 3 attacks.
        // Make it more general.
        private readonly AIAttack _jumpAttack;
        private readonly AIAttack _hornAttack;
        private readonly AIAttack _billAttack;

        public RhinbillCombatState(AIStateMachine stateMachine, List<Transform> detectedTargets, Transform targetReceived) : base(stateMachine)
        {
            _detectedTargets = detectedTargets;
            _targetReceived = targetReceived;
            
            _jumpAttack = StateMachine.AIFighter.GetAttack(AIAttackNames.JumpAttack);
            _hornAttack = StateMachine.AIFighter.GetAttack(AIAttackNames.HornAttack);
            _billAttack = StateMachine.AIFighter.GetAttack(AIAttackNames.BillAttack);
        }
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionStateHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AIMover.SwitchMovementToNavmesh();
        }

        public override void Tick()
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 0f, StateMachine.AnimatorDampTime, Time.deltaTime);
            
            Transform target = _detectedTargets.Contains(_targetReceived) ? _targetReceived : null;

            if (target == null)
            {
                StateMachine.SwitchState(new AISuspicionState(StateMachine));
                return;
            }
            
            if (!StateMachine.AIFighter.ReadyForNextAttack()) return;

            if (IsInAttackRange(target.position, _hornAttack.Range))
            {
                PerformAttack(target, _hornAttack);
                return;
            }

            if (IsInAttackRange(target.position, _billAttack.Range))
            {
                PerformAttack(target, _billAttack);
                return;
            }

            if (IsInAttackRange(target.position, _jumpAttack.Range))
            {
                PerformAttack(target, _jumpAttack);
                return;
            }
            
            StateMachine.SwitchState(new AIChasingState(StateMachine, _detectedTargets, _jumpAttack.Range));
            return;
        }

        public override void Exit()
        {
        }

        private void PerformAttack(Transform target, AIAttack attack)
        {
            StateMachine.AIFighter.ResetTimer();
            StateMachine.AIFighter.StartTimer();
            Vector3 directionTowardsTarget = target.position - StateMachine.transform.position;

            StateMachine.SwitchState
            (
                new AIRotationState
                (
                    StateMachine,
                    directionTowardsTarget,
                    new AIAttackingState(StateMachine, attack)
                )
            );
        }
    }
}
