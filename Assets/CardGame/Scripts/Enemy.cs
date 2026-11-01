using System;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("maxHealth")] private int _maxHealth = 50;
    [SerializeField, FormerlySerializedAs("attackDamage")] private int _attackDamage = 5;
    [SerializeField, FormerlySerializedAs("healthText")] private TMP_Text _healthText;
    [SerializeField, FormerlySerializedAs("poisonText")] private TMP_Text _poisonText;

    private int _currentHealth;
    private int _poisonStack;

    public bool IsDead => _currentHealth <= 0;
    public event Action OnHealthChanged;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _poisonStack = 0;

        UpdateUI();
    }

    public void Attack(Player player)
    {
        if (_currentHealth <= 0)
            return;

        player.TakeDamage(_attackDamage);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - damage);

        UpdateUI();

        OnHealthChanged?.Invoke();
    }



    public void AddPoison(int amount)
    {
        _poisonStack += Mathf.Max(0, amount);
        UpdateUI();
    }

    public void ProcessPoison()
    {
        if (_poisonStack <= 0 || _currentHealth <= 0)
            return;

        TakeDamage(_poisonStack);

        _poisonStack = Mathf.Max(0, _poisonStack - 1);

        UpdateUI();
    }

    private void UpdateUI()
    {
        _healthText.text = $"{_currentHealth} / {_maxHealth}";
        _poisonText.text = $"중독: {_poisonStack}";
    }
}
