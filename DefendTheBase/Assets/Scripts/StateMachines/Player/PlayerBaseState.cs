using Combat;
using UnityEngine;

namespace StateMachines.Player
{
    public abstract class PlayerBaseState : State
    {
        protected readonly PlayerStateMachine StateMachine;
        
        protected PlayerBaseState(PlayerStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public override void Tick(float deltaTime)
        {
            StateMachine.PlayerMover.FaceCameraForward(deltaTime);
        }
        
        protected bool HasAnimationFinished(string tag)
        {
            return GetNormalizedAnimationTime(StateMachine.Animator, tag) >= 1f;
        }
        
        /// <summary>
        /// Normalized time:
        /// - The integer part is the number of time a state has been looped.
        /// - The fractional part is the % (0-1) of progress in the current loop.
        ///
        /// It gets the normalized time of currently played animation tagged with tag.
        /// </summary>
        /// <returns></returns>
        protected float GetNormalizedAnimationTime(Animator animator, string tag)
        {
            AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

            if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
            {
                return nextInfo.normalizedTime;
            }

            if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
            {
                return currentInfo.normalizedTime;
            }

            return 0f;
        }
    }
}
