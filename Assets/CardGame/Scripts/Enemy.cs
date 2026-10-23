using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private TMP_Text healthText;

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthText.text = $"{currentHealth} / {maxHealth}";
    }
}
