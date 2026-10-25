using UnityEngine;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private TMP_Text healthText;

    private int currentHealth;

    public event Action OnHealthChanged;
    public bool IsDead => currentHealth <= 0;

    private void Start()
    {
        currentHealth = 30;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);

        UpdateHealthUI();

        OnHealthChanged?.Invoke();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        UpdateHealthUI();

        OnHealthChanged?.Invoke();
    }


    private void UpdateHealthUI()
    {
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

}
