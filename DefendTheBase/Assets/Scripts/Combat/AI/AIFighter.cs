using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Combat.AI
{
    public class AIFighter : MonoBehaviour
    {
        [SerializeField] private WeaponController equippedBill;
        [SerializeField] private WeaponController equippedHorn;
        [SerializeField] private AIAttack[] aiAttacks = new AIAttack[3];
        [SerializeField] private float delayBetweenAttacks = 1f;

        private float _timeToNextAttack;
        private Coroutine _timerCoroutine;

        public float MaxAttackRange { get; private set; }

        private void Start()
        {
            MaxAttackRange = GetRangeOfMostRangedAttack();
        }

        public void ResetTimer()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
            }
            
            _timeToNextAttack = delayBetweenAttacks;
        }

        public bool ReadyForNextAttack()
        {
            return _timeToNextAttack <= 0f;
        }

        public void StartTimer()
        {
            _timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        public AIAttack GetAttack(AIAttackNames aiAttackName)
        {
            return aiAttacks.FirstOrDefault(attack => attack.AIAttackName == aiAttackName);
        }

        public List<AIAttack> GetAvailableAttacks(Vector3 targetPosition)
        {
            return aiAttacks.Where(attack => IsInAttackRange(targetPosition, attack.Range)).ToList();
        }

        public bool IsInAttackRange(Vector3 targetPosition, float attackRange)
        {
            return (targetPosition - transform.position).sqrMagnitude
                   <= Mathf.Pow(attackRange, 2);
        }

        private float GetRangeOfMostRangedAttack()
        {
            return aiAttacks.Aggregate(0f, (current, attack) => Mathf.Max(current, attack.Range));
        }

        private IEnumerator TimerCoroutine()
        {
            while (_timeToNextAttack > 0f)
            {
                _timeToNextAttack -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        
        // Animation event methods
        public void EnableBillDamager() => equippedBill.EnableDamageTrigger();
        public void DisableBillDamager() => equippedBill.DisableDamageTrigger();
        public void EnableHornDamager() => equippedHorn.EnableDamageTrigger();
        public void DisableHornDamager() => equippedHorn.DisableDamageTrigger();
    }
}
