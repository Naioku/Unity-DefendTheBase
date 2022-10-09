using System.Collections;
using System.Linq;
using Core;
using UnityEngine;

namespace Combat.AI
{
    public class AIFighter : MonoBehaviour
    {
        [SerializeField] public AIAttack[] aiAttacks = new AIAttack[3];
        [SerializeField] public float delayBetweenAttacks = 1f;
        
        private float _timeToNextAttack;
        private Coroutine _timerCoroutine;

        public AIAttack GetAttack(AIAttackNames aiAttackName)
        {
            return aiAttacks.FirstOrDefault(attack => attack.AIAttackName == aiAttackName);
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
        
        private IEnumerator TimerCoroutine()
        {
            while (_timeToNextAttack > 0f)
            {
                _timeToNextAttack -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
