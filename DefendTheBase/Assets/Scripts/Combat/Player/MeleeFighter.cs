using System.Linq;
using Core;
using UnityEngine;

namespace Combat.Player
{
    public class MeleeFighter : MonoBehaviour
    {
        [SerializeField] private MeleeAttack[] attacks = new MeleeAttack[4];
        [SerializeField] private WeaponController equippedMainHandWeapon;
        [SerializeField] private WeaponController equippedOffHandWeapon;

        public MeleeAttack GetAttack(MeleeAttackNames meleeAttackName)
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
