using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using TMPro;

public class Enemy : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EnemyData _data;
    [SerializeField, FormerlySerializedAs("healthText")] private TMP_Text _healthText;
    [SerializeField, FormerlySerializedAs("poisonText")] private TMP_Text _poisonText;
    [SerializeField] private TMP_Text _blockText;
    [SerializeField] private TMP_Text _intentText;

    private int _currentHealth;

    // 다음 턴에 수행할 패턴 순번
    private int _moveIndex;

    public bool IsDead => _currentHealth <= 0;
    public event Action OnHealthChanged;
    public event Action<Enemy> OnClicked;
    public CombatStatus Status { get; } = new CombatStatus();

    private EnemyMove CurrentMove => _data.Moves[_moveIndex];

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
        _currentHealth = _data.MaxHealth;
        _moveIndex = 0;

        UpdateUI();
        UpdateIntentUI();
    }

    // 현재 순번의 행동 묶음을 수행하고 다음 순번으로 이동
    public void PerformMove(Player target)
    {
        if (_currentHealth <= 0)
            return;

        foreach (EnemyActionEntry entry in CurrentMove.Actions)
        {
            if (entry.Action == null)
                continue;

            entry.Action.Execute(this, target, entry.Value);
        }

        _moveIndex = (_moveIndex + 1) % _data.Moves.Count;

        UpdateIntentUI();
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

        // 처치된 적은 화면에서 숨김
        if (IsDead)
            gameObject.SetActive(false);
    }

    // 대상 지정을 위한 클릭 입력
    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsDead)
            return;

        OnClicked?.Invoke(this);
    }

    private void UpdateUI()
    {
        _healthText.text = $"{_currentHealth} / {_data.MaxHealth}";
        _poisonText.text = $"중독: {Status.Poison}";
        _blockText.text = $"방어도: {Status.Block}";
    }

    private void UpdateIntentUI()
    {
        _intentText.text = $"의도: {CurrentMove.GetIntentText()}";
    }
}
