using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class WeaponDamageTrigger : MonoBehaviour
    {
        private float _damage;
        private Collider _ownerCollider;
        private readonly List<Collider> _alreadyCollidedWith = new();

        private void OnEnable()
        {
            _alreadyCollidedWith.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == _ownerCollider) return;

            if (_alreadyCollidedWith.Contains(other)) return;
            _alreadyCollidedWith.Add(other);

            if (!other.TryGetComponent(out Health health)) return;
            health.TakeDamage(_damage);
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }
        
        public void SetOwnerCollider(Collider collider)
        {
            _ownerCollider = collider;
        }
    }
}
