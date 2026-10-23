using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private TMP_Text healthText;

    private int currentHealth;

    private void Start()
    {
        currentHealth = 30;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

}
