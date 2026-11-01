
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
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
    [SerializeField] private Button _continueButton;
    [SerializeField] private TMP_Text _continueButtonText;

    // 계속 버튼을 눌렀을 때 이동할 씬
    private string _nextSceneName;

    public BattleState CurrentState { get; private set; }

    public bool IsBattleActive =>
        CurrentState == BattleState.Playing;

    public IReadOnlyList<Enemy> Enemies => _enemies;

    public event Action OnBattleEnded;

    private void Awake()
    {
        CurrentState = BattleState.Playing;
        _resultText.text = "";
        _continueButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        _continueButton.onClick.AddListener(LoadNextScene);
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

        RecordBattleResult();
        ShowContinueButton();

        OnBattleEnded?.Invoke();
    }

    // 승리 시 남은 체력과 승리 수를 런에 기록
    private void RecordBattleResult()
    {
        if (CurrentState == BattleState.Victory)
            RunManager.Instance.CompleteBattle(_player.CurrentHealth);
    }

    // 다음 전투가 남았으면 전투 씬, 클리어 또는 패배면 결과 씬으로 이동
    private void ShowContinueButton()
    {
        bool hasNextBattle =
            CurrentState == BattleState.Victory &&
            !RunManager.Instance.IsRunCleared;

        _nextSceneName = hasNextBattle ? SceneNames.Battle : SceneNames.Result;
        _continueButtonText.text = hasNextBattle ? "다음 전투" : "결과 보기";

        _continueButton.gameObject.SetActive(true);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(_nextSceneName);
    }

    private void OnDestroy()
    {
        if (_continueButton != null)
        {
            _continueButton.onClick.RemoveListener(LoadNextScene);
        }
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
