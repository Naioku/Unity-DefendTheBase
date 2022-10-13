using Combat;
using TMPro;
using UnityEngine;

namespace Stats
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Health health;
        
        private TextMeshProUGUI _label;

        private void Awake()
        {
            _label = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            health.TakeDamageEventWithHealthValue += HandleTakeDamage;
        }
        
        private void OnDisable()
        {
            health.TakeDamageEventWithHealthValue -= HandleTakeDamage;
        }

        private void HandleTakeDamage(float value)
        {
            _label.text = value.ToString();
        }
    }
}
