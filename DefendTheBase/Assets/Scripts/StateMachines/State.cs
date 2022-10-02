using UnityEngine;

namespace StateMachines
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick();
        public abstract void Exit();

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
