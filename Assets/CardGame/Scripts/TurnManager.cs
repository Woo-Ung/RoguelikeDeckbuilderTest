using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public enum TurnState
    {
        Player,
        Enemy
    }

    [SerializeField] private Player player;
    [SerializeField] private Enemy enemy;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TMP_Text turnText;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private PlayerEnergy playerEnergy;

    public event Action OnPlayerTurnStarted;
    public event Action OnPlayerTurnEnded;

    public TurnState CurrentTurn { get; private set; }

    public bool IsPlayerTurn =>
        CurrentTurn == TurnState.Player;


    private void OnEnable()
    {
        battleManager.OnBattleEnded += EndBattle;
    }

    private void OnDisable()
    {
        battleManager.OnBattleEnded -= EndBattle;
    }


    private void Start()
    {
        endTurnButton.onClick.AddListener(EndPlayerTurn);

        StartPlayerTurn();
    }


    private void StartPlayerTurn()
    {
        if (!battleManager.IsBattleActive)
            return;

        CurrentTurn = TurnState.Player;

        playerEnergy.RestoreEnergy();

        turnText.text = "플레이어 턴";
        endTurnButton.interactable = true;

        OnPlayerTurnStarted?.Invoke();
    }


    public void EndPlayerTurn()
    {
        if (!battleManager.IsBattleActive || !IsPlayerTurn)
            return;

        OnPlayerTurnEnded?.Invoke();

        CurrentTurn = TurnState.Enemy;

        turnText.text = "적 턴";
        endTurnButton.interactable = false;

        ProcessEnemyTurn();
    }

    private void ProcessEnemyTurn()
    {
        enemy.ProcessPoison();

        if (!battleManager.IsBattleActive)
            return;

        enemy.Attack(player);

        if (!battleManager.IsBattleActive)
            return;

        StartPlayerTurn();
    }



    public void EndBattle()
    {
        endTurnButton.interactable = false;

        turnText.text = "전투 종료";
    }


    private void OnDestroy()
    {
        if (endTurnButton != null)
        {
            endTurnButton.onClick.RemoveListener(EndPlayerTurn);
        }
    }
}
