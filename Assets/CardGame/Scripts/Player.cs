using UnityEngine;
using UnityEngine.Serialization;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("healthText")] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _blockText;
    [SerializeField] private TMP_Text _poisonText;

    private int _maxHealth;
    private int _currentHealth;

    public event Action OnHealthChanged;
    public bool IsDead => _currentHealth <= 0;
    public int CurrentHealth => _currentHealth;
    public CombatStatus Status { get; } = new CombatStatus();

    private void OnEnable()
    {
        Status.OnStatusChanged += UpdateStatusUI;
    }

    private void OnDisable()
    {
        Status.OnStatusChanged -= UpdateStatusUI;
    }

    private void Start()
    {
        // 체력은 런 데이터에서 이어받음
        _maxHealth = RunManager.Instance.MaxHealth;
        _currentHealth = RunManager.Instance.CurrentHealth;
        UpdateHealthUI();
        UpdateStatusUI();
    }

    public void TakeDamage(int damage)
    {
        int remainingDamage = Status.AbsorbDamage(damage);

        LoseHealth(remainingDamage);
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);

        UpdateHealthUI();

        OnHealthChanged?.Invoke();
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

        UpdateHealthUI();

        OnHealthChanged?.Invoke();
    }


    private void UpdateHealthUI()
    {
        _healthText.text = $"{_currentHealth} / {_maxHealth}";
    }

    private void UpdateStatusUI()
    {
        _blockText.text = $"방어도: {Status.Block}";
        _poisonText.text = $"중독: {Status.Poison}";
    }

}
