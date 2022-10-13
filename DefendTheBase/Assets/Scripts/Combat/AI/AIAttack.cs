using System;
using Core;
using UnityEngine;

namespace Combat.AI
{
    [Serializable]
    public class AIAttack : IComparable<AIAttack>
    {
        [field: SerializeField] public AIAttackNames AIAttackName { get; private set; }
        [field: SerializeField] public string AnimationName { get; private set; }
        [field: SerializeField] public float TransitionDuration { get; private set; } = 0.1f;
        [field: SerializeField] public float Range { get; private set; } = 2f;
        [field: SerializeField] public float AttackerDisplacement { get; private set; } = 0f;
        [field: SerializeField] public float AttackerDisplacementSmoothingTime { get; private set; } = 0.1f;
        
        [field: Range(0f, 1f)]
        [field: SerializeField] public float DisplacementApplicationNormalizedTime { get; private set; } = 0.1f;

        public int CompareTo(AIAttack other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Range.CompareTo(other.Range);
        }
    }
}