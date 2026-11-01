
using System;
using System.Collections.Generic;
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
    [SerializeField] private Enemy[] _enemies;
    [SerializeField, FormerlySerializedAs("resultText")] private TMP_Text _resultText;

    public BattleState CurrentState { get; private set; }

    public bool IsBattleActive =>
        CurrentState == BattleState.Playing;

    public IReadOnlyList<Enemy> Enemies => _enemies;

    public event Action OnBattleEnded;

    private void Awake()
    {
        CurrentState = BattleState.Playing;
        _resultText.text = "";
    }

    private void OnEnable()
    {
        _player.OnHealthChanged += CheckBattleResult;

        foreach (Enemy enemy in _enemies)
        {
            enemy.OnHealthChanged += CheckBattleResult;
        }
    }

    private void OnDisable()
    {
        _player.OnHealthChanged -= CheckBattleResult;

        foreach (Enemy enemy in _enemies)
        {
            enemy.OnHealthChanged -= CheckBattleResult;
        }
    }

    public void CheckBattleResult()
    {
        if (!IsBattleActive)
            return;

        if (AreAllEnemiesDead())
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

    private bool AreAllEnemiesDead()
    {
        foreach (Enemy enemy in _enemies)
        {
            if (!enemy.IsDead)
                return false;
        }

        return true;
    }
}
