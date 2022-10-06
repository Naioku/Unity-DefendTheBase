using UnityEngine;

namespace Combat.Player
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponDamageTrigger damageTrigger;
        [SerializeField] private Collider ownerCollider;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float knockback = 5f;

        private void Start()
        {
            damageTrigger.SetOwnerCollider(ownerCollider);
            damageTrigger.SetDamage(damage);
            damageTrigger.SetKnockback(knockback);
            DisableDamageTrigger();
        }

        public void EnableDamageTrigger()
        {
            damageTrigger.gameObject.SetActive(true);
        }
        
        public void DisableDamageTrigger()
        {
            damageTrigger.gameObject.SetActive(false);
        }
    }
}
