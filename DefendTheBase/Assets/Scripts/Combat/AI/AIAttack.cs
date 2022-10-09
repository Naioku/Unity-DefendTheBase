using System;
using Core;
using UnityEngine;

namespace Combat.AI
{
    [Serializable]
    public class AIAttack
    {
        [field: SerializeField]
        public AIAttackNames AIAttackName { get; private set; }
        
        [field: SerializeField]
        public string AnimationName { get; private set; }
        
        [field: SerializeField]
        public float TransitionDuration { get; private set; }
    }
}