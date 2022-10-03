using System;
using Core;
using UnityEngine;

namespace Combat
{
    [Serializable]
    public class Attack
    {
        [field: SerializeField]
        public MeleeAttackNames MeleeAttackName { get; private set; }
        
        [field: SerializeField]
        public string AnimationName { get; private set; }
        
        [field: SerializeField]
        public float TransitionDuration { get; private set; }

        [field: Tooltip("In/after what fraction of the animation time next attack can be performed? " +
                        "In/after what time player should click to perform next combo attack?")]
        [field: Range(0f, 1f)]
        [field: SerializeField]
        public float NextComboAttackNormalizedTime { get; private set; }

        [field: Tooltip("To what time player can cancel an attack.")]
        [field: Range(0f, 1f)]
        [field: SerializeField]
        public float CancelAttackNormalizedTime { get; private set; } = 0.1f;
    }
}
