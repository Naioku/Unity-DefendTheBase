using System;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        public event Action<Vector3> TakeDamageEvent;
        public event Action TakeHitEvent;
        public bool IsVulnerable { get; set; } = true;
        
        [SerializeField] private float health = 100f;

        public void TakeDamage(float damage, Vector3 hitDirection)
        {
            if (!IsVulnerable)
            {
                TakeHitEvent?.Invoke();
                return;
            }
            
            health = Mathf.Max(0f, health - damage);
            TakeDamageEvent?.Invoke(hitDirection);

            if (health == 0f)
            {
                print("Character is dead xp");
            }
        }
    }
}
