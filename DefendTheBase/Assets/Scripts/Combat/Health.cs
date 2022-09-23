using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(0f, health - damage);

            if (health == 0f)
            {
                print("Character is dead xp");
            }
        }
    }
}
