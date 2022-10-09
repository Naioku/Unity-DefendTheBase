using System;
using Core;
using UnityEngine;

namespace Combat.AI
{
    [Serializable]
    public class AIAttack
    {
        [field: SerializeField] public AIAttackNames AIAttackName { get; private set; }
        [field: SerializeField] public string AnimationName { get; private set; }
        [field: SerializeField] public float TransitionDuration { get; private set; } = 0.1f;
        [field: SerializeField] public float Range { get; private set; } = 2f;
        [field: SerializeField] public float AttackerDisplacement { get; private set; } = 0f;
        [field: SerializeField] public float AttackerImpactSmoothingTime { get; private set; } = 0.1f;
        
        [field: Range(0f, 1f)]
        [field: SerializeField] public float ForceApplicationNormalizedTime { get; private set; } = 0.1f;
    }
}