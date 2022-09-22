using System.Linq;
using UnityEngine;

namespace Combat
{
    public class MeleeFighter : MonoBehaviour
    {
        [SerializeField] private Attack[] _attacks = new Attack[4];

        public Attack GetAttack(AttackNames attackName)
        {
            return _attacks.FirstOrDefault(attack => attack.AttackName == attackName);
        }
    }

    public enum AttackNames
    {
        Left,
        Right,
        Backward,
        Forward
    }
}
