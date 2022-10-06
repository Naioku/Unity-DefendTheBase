using System.Collections.Generic;
using Locomotion;
using UnityEngine;

namespace Combat.Player
{
    public class WeaponDamageTrigger : MonoBehaviour
    {
        private Collider _ownerCollider;
        private float _damage;
        private float _knockbackValue;
        
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
            health.TakeDamage(_damage, GetHitDirection(other));
            
            if (!other.TryGetComponent(out ForceReceiver forceReceiver)) return;
            forceReceiver.AddForce(GetHitDirection(other) * _knockbackValue);
        }

        public void SetOwnerCollider(Collider collider)
        {
            _ownerCollider = collider;
        }

        public void SetDamage(float value)
        {
            _damage = value;
        }

        public void SetKnockback(float value)
        {
            _knockbackValue = value;
        }

        private Vector3 GetHitDirection(Collider other)
        {
            return (other.transform.position - _ownerCollider.transform.position).normalized;
        }
    }
}
