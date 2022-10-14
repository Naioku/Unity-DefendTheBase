using System;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        public event Action<Vector3> TakeDamageEvent;
        public event Action TakeHitEvent;
        public event Action DeathEvent;
        
        public bool IsVulnerable { get; set; } = true;
        
        [field: SerializeField] public float HealthValue { get; private set; } = 100f;

        public void TakeDamage(float damage, Vector3 hitDirection)
        {
            if (!IsVulnerable)
            {
                TakeHitEvent?.Invoke();
                return;
            }
            
            HealthValue = Mathf.Max(0f, HealthValue - damage);
            TakeDamageEvent?.Invoke(hitDirection);

            if (HealthValue == 0f)
            {
                DeathEvent?.Invoke();
                print("Character is dead xp");
            }
        }
    }
}
