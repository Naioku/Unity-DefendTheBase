using UnityEngine;

namespace Combat
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponDamageTrigger damageTrigger;
        [SerializeField] private float damage = 10f;
        [SerializeField] private Collider ownerCollider;

        private void Start()
        {
            damageTrigger.SetDamage(damage);
            damageTrigger.SetOwnerCollider(ownerCollider);
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
