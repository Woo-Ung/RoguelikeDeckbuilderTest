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

    public bool IsDead => _currentHealth <= 0;
    public event Action OnHealthChanged;
    public CombatStatus Status { get; } = new CombatStatus();

    private void OnEnable()
    {
        Status.OnStatusChanged += UpdateUI;
    }

    private void OnDisable()
    {
        Status.OnStatusChanged -= UpdateUI;
    }

    private void Start()
    {
        _currentHealth = _maxHealth;

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
        int remainingDamage = Status.AbsorbDamage(damage);

        LoseHealth(remainingDamage);
    }



    public void AddPoison(int amount)
    {
        Status.AddPoison(amount);
    }

    // 자기 턴 시작 시 방어도 초기화 및 중독 처리
    public void StartTurn()
    {
        Status.ResetBlock();
        ProcessPoison();
    }

    private void ProcessPoison()
    {
        if (IsDead)
            return;

        int poisonDamage = Status.ConsumePoison();

        if (poisonDamage > 0)
            LoseHealth(poisonDamage);
    }

    // 방어도를 거치지 않고 체력 감소
    private void LoseHealth(int amount)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - amount);

        UpdateUI();

        OnHealthChanged?.Invoke();
    }

    private void UpdateUI()
    {
        _healthText.text = $"{_currentHealth} / {_maxHealth}";
        _poisonText.text = $"중독: {Status.Poison}";
    }
}
