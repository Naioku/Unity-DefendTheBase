using System.Linq;
using Core;
using UnityEngine;

namespace Combat.Player
{
    public class MeleeFighter : MonoBehaviour
    {
        [SerializeField] private Attack[] attacks = new Attack[4];
        [SerializeField] private WeaponController equippedMainHandWeapon;
        [SerializeField] private WeaponController equippedOffHandWeapon;

        public Attack GetAttack(MeleeAttackNames meleeAttackName)
        {
            return attacks.FirstOrDefault(attack => attack.MeleeAttackName == meleeAttackName);
        }
        
        // Animation event methods
        public void EnableWeaponMainHandDamager() => equippedMainHandWeapon.EnableDamageTrigger();
        public void DisableWeaponMainHandDamager() => equippedMainHandWeapon.DisableDamageTrigger();
        public void EnableWeaponOffHandDamager() => equippedOffHandWeapon.EnableDamageTrigger();
        public void DisableWeaponOffHandDamager() => equippedOffHandWeapon.DisableDamageTrigger();

    }
}
