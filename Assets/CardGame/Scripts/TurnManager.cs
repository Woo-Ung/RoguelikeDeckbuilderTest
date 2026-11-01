using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public enum TurnState
    {
        Player,
        Enemy
    }

    [SerializeField, FormerlySerializedAs("player")] private Player _player;
    [SerializeField, FormerlySerializedAs("enemy")] private Enemy _enemy;
    [SerializeField, FormerlySerializedAs("endTurnButton")] private Button _endTurnButton;
    [SerializeField, FormerlySerializedAs("turnText")] private TMP_Text _turnText;
    [SerializeField, FormerlySerializedAs("battleManager")] private BattleManager _battleManager;
    [SerializeField, FormerlySerializedAs("playerEnergy")] private PlayerEnergy _playerEnergy;

    public event Action OnPlayerTurnStarted;
    public event Action OnPlayerTurnEnded;

    public TurnState CurrentTurn { get; private set; }

    public bool IsPlayerTurn =>
        CurrentTurn == TurnState.Player;


    private void OnEnable()
    {
        _battleManager.OnBattleEnded += EndBattle;
    }

    private void OnDisable()
    {
        _battleManager.OnBattleEnded -= EndBattle;
    }


    private void Start()
    {
        _endTurnButton.onClick.AddListener(EndPlayerTurn);

        StartPlayerTurn();
    }


    private void StartPlayerTurn()
    {
        if (!_battleManager.IsBattleActive)
            return;

        CurrentTurn = TurnState.Player;

        _playerEnergy.RestoreEnergy();

        _turnText.text = "플레이어 턴";
        _endTurnButton.interactable = true;

        OnPlayerTurnStarted?.Invoke();
    }


    public void EndPlayerTurn()
    {
        if (!_battleManager.IsBattleActive || !IsPlayerTurn)
            return;

        OnPlayerTurnEnded?.Invoke();

        CurrentTurn = TurnState.Enemy;

        _turnText.text = "적 턴";
        _endTurnButton.interactable = false;

        ProcessEnemyTurn();
    }

    private void ProcessEnemyTurn()
    {
        _enemy.ProcessPoison();

        if (!_battleManager.IsBattleActive)
            return;

        _enemy.Attack(_player);

        if (!_battleManager.IsBattleActive)
            return;

        StartPlayerTurn();
    }



    public void EndBattle()
    {
        _endTurnButton.interactable = false;

        _turnText.text = "전투 종료";
    }


    private void OnDestroy()
    {
        if (_endTurnButton != null)
        {
            _endTurnButton.onClick.RemoveListener(EndPlayerTurn);
        }
    }
}
