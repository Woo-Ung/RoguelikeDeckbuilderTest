
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text poisonText;

    private int currentHealth;
    private int poisonStack;

    private void Start()
    {
        currentHealth = maxHealth;
        poisonStack = 0;

        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateUI();
    }

    public void AddPoison(int amount)
    {
        poisonStack += Mathf.Max(0, amount);
        UpdateUI();
    }

    public void ProcessPoison()
    {
        if (poisonStack <= 0 || currentHealth <= 0)
            return;

        TakeDamage(poisonStack);

        poisonStack = Mathf.Max(0, poisonStack - 1);

        UpdateUI();
    }

    private void UpdateUI()
    {
        healthText.text = $"{currentHealth} / {maxHealth}";
        poisonText.text = $"중독: {poisonStack}";
    }
}
