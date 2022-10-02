using System;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        public event Action<Vector3> OnTakeDamage;
        public bool IsVulnerable { get; set; } = true;
        
        [SerializeField] private float health = 100f;

        public void TakeDamage(float damage, Vector3 hitDirection)
        {
            if (!IsVulnerable) return;
            
            health = Mathf.Max(0f, health - damage);
            OnTakeDamage?.Invoke(hitDirection);

            if (health == 0f)
            {
                print("Character is dead xp");
            }
        }
    }
}
