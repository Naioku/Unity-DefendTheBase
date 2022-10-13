using StateMachines.AI.Rhinbill;
using UnityEngine;

namespace StateMachines.AI
{
    public class AIChasingState : AIBaseState
    {
        private static readonly int LocomotionStateHash = Animator.StringToHash("Locomotion");
        private static readonly int ForwardMovementSpeedHash = Animator.StringToHash("ForwardMovementSpeed");
        
        private readonly float _targetStopRange;

        private Vector3 _lastSeenTargetPosition;

        public AIChasingState(AIStateMachine stateMachine, float targetStopRange) : base(stateMachine)
        {
            _targetStopRange = targetStopRange;
        }
        
        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(LocomotionStateHash, StateMachine.AnimationCrossFadeDuration);
            StateMachine.AIMover.SwitchMovementToNavmesh();
        }

        public override void Tick()
        {
            StateMachine.Animator.SetFloat(ForwardMovementSpeedHash, 1f, StateMachine.AnimatorDampTime, Time.deltaTime);

            if (!StateMachine.FocusOnTarget)
            {
                StateMachine.CurrentTarget = GetClosestReachableTarget();
            }

            if (StateMachine.CurrentTarget == null)
            {
                if (!StateMachine.AIMover.MoveToPosition(_lastSeenTargetPosition) ||
                    IsDestinationReached(_lastSeenTargetPosition, StateMachine.WaypointTolerance))
                {
                    StateMachine.SwitchState(new AISuspicionState(StateMachine));
                    return;
                }

                return;
            }
            
            _lastSeenTargetPosition = StateMachine.CurrentTarget.position;
            
            if (StateMachine.AIFighter.IsInAttackRange(_lastSeenTargetPosition, _targetStopRange))
            {
                StateMachine.SwitchState(new RhinbillCombatState(StateMachine));
                return;
            }
            
            if (!StateMachine.AIMover.MoveToPosition(_lastSeenTargetPosition))
            {
                StateMachine.SwitchState(new AISuspicionState(StateMachine));
                return;
            }
        }

        public override void Exit()
        {
            StateMachine.AIMover.StopMovement();
        }
    }
}
