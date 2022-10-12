using System.Collections.Generic;
using System.Linq;
using Combat.AI;
using UnityEngine;

namespace StateMachines.AI.Rhinbill
{
    public class RhinbillCombatState : AIBaseState
    {
        private static readonly int LocomotionStateHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        private readonly List<Transform> _detectedTargets;
        private readonly Transform _targetReceived;
        private readonly System.Random _randomNumberGenerator = new();

        public RhinbillCombatState(AIStateMachine stateMachine, List<Transform> detectedTargets, Transform targetReceived) : base(stateMachine)
        {
            _detectedTargets = detectedTargets;
            _targetReceived = targetReceived;
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

            if (!StateMachine.AIFighter.IsInAttackRange(target.position, StateMachine.AIFighter.MaxAttackRange))
            {
                StateMachine.SwitchState(new AIChasingState(StateMachine, _detectedTargets, StateMachine.AIFighter.MaxAttackRange));
                return;
            }
            
            if (!StateMachine.AIFighter.ReadyForNextAttack()) return;

            List<AIAttack> availableAttacks = StateMachine.AIFighter.GetAvailableAttacks(target.position);

            if (availableAttacks.Count == 0)
            {
                Debug.Log("RhinbillCombatState BUG: No attack available.");
                return;
            }
            
            availableAttacks.Sort();
            var attacksWithClosestRange = GetAttacksWithClosestRange(availableAttacks);
            PerformAttack(target, GetRandomAttack(attacksWithClosestRange));
            return;
        }

        public override void Exit()
        {
        }

        private List<AIAttack> GetAttacksWithClosestRange(List<AIAttack> attacks)
        {
            float closestAttackRange = attacks[0].Range;
            return attacks.TakeWhile(attack => !(attack.Range > closestAttackRange)).ToList();
        }

        private AIAttack GetRandomAttack(List<AIAttack> attacks)
        {
            int attackNumber = _randomNumberGenerator.Next(0, attacks.Count);
            return attacks[attackNumber];
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
