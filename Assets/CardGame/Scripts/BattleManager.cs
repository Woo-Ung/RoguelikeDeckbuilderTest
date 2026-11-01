
using System;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public enum BattleState
    {
        Playing,
        Victory,
        Defeat
    }

    [SerializeField, FormerlySerializedAs("player")] private Player _player;
    [SerializeField, FormerlySerializedAs("enemy")] private Enemy _enemy;
    [SerializeField, FormerlySerializedAs("resultText")] private TMP_Text _resultText;

    public BattleState CurrentState { get; private set; }

    public bool IsBattleActive =>
        CurrentState == BattleState.Playing;

    public event Action OnBattleEnded;

    private void Awake()
    {
        CurrentState = BattleState.Playing;
        _resultText.text = "";
    }

    private void OnEnable()
    {
        _player.OnHealthChanged += CheckBattleResult;
        _enemy.OnHealthChanged += CheckBattleResult;
    }

    private void OnDisable()
    {
        _player.OnHealthChanged -= CheckBattleResult;
        _enemy.OnHealthChanged -= CheckBattleResult;
    }

    public void CheckBattleResult()
    {
        if (!IsBattleActive)
            return;

        if (_enemy.IsDead)
        {
            CurrentState = BattleState.Victory;
            _resultText.text = "승리!";
        }
        else if (_player.IsDead)
        {
            CurrentState = BattleState.Defeat;
            _resultText.text = "패배!";
        }
        else
        {
            return;
        }

        OnBattleEnded?.Invoke();
    }
}
