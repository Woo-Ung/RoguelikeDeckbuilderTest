using UnityEngine;
using UnityEngine.Serialization;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("maxHealth")] private int _maxHealth = 50;
    [SerializeField, FormerlySerializedAs("healthText")] private TMP_Text _healthText;

    private int _currentHealth;

    public event Action OnHealthChanged;
    public bool IsDead => _currentHealth <= 0;

    private void Start()
    {
        _currentHealth = _maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - damage);

        UpdateHealthUI();

        OnHealthChanged?.Invoke();
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);

        UpdateHealthUI();

        OnHealthChanged?.Invoke();
    }


    private void UpdateHealthUI()
    {
        _healthText.text = $"{_currentHealth} / {_maxHealth}";
    }

}
