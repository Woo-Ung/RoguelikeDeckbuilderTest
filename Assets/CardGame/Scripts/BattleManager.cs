
using System;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public enum BattleState
    {
        Playing,
        Victory,
        Defeat
    }

    [SerializeField] private Player player;
    [SerializeField] private Enemy enemy;
    [SerializeField] private TMP_Text resultText;

    public BattleState CurrentState { get; private set; }

    public bool IsBattleActive =>
        CurrentState == BattleState.Playing;

    public event Action OnBattleEnded;

    private void Awake()
    {
        CurrentState = BattleState.Playing;
        resultText.text = "";
    }

    private void OnEnable()
    {
        player.OnHealthChanged += CheckBattleResult;
        enemy.OnHealthChanged += CheckBattleResult;
    }

    private void OnDisable()
    {
        player.OnHealthChanged -= CheckBattleResult;
        enemy.OnHealthChanged -= CheckBattleResult;
    }

    public void CheckBattleResult()
    {
        if (!IsBattleActive)
            return;

        if (enemy.IsDead)
        {
            CurrentState = BattleState.Victory;
            resultText.text = "승리!";
        }
        else if (player.IsDead)
        {
            CurrentState = BattleState.Defeat;
            resultText.text = "패배!";
        }
        else
        {
            return;
        }

        OnBattleEnded?.Invoke();
    }
}
