using System;
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

        private void Start()
        {
            UpdateLabel();
        }

        private void OnEnable()
        {
            health.TakeDamageEvent += HandleTakeDamage;
        }

        private void OnDisable()
        {
            health.TakeDamageEvent -= HandleTakeDamage;
        }

        private void HandleTakeDamage(Vector3 direction)
        {
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            _label.text = health.HealthValue.ToString();
        }
    }
}
