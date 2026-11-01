
using System.Collections.Generic;
using UnityEngine;

// 전투 사이에 유지되는 런 데이터 관리
// 다른 스크립트의 Awake에서 런 데이터를 읽을 수 있도록 먼저 실행
[DefaultExecutionOrder(-100)]
public class RunManager : MonoBehaviour
{
    [Header("Run Settings")]
    [SerializeField] private CardData[] _startingDeck;
    [SerializeField] private int _startingMaxHealth = 50;

    // 클리어에 필요한 전투 승리 수 (보상·맵 구현 전 임시)
    [SerializeField] private int _battlesToClear = 3;

    private readonly List<CardData> _deck = new();

    public static RunManager Instance { get; private set; }

    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public int BattlesWon { get; private set; }
    public IReadOnlyList<CardData> Deck => _deck;
    public bool IsRunCleared => BattlesWon >= _battlesToClear;

    private void Awake()
    {
        // 씬마다 배치된 런 관리자 중 처음 생성된 것만 유지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartNewRun();
    }

    public void StartNewRun()
    {
        MaxHealth = _startingMaxHealth;
        CurrentHealth = MaxHealth;
        BattlesWon = 0;

        _deck.Clear();
        _deck.AddRange(_startingDeck);
    }

    // 전투 승리 기록 및 남은 체력 저장
    public void CompleteBattle(int remainingHealth)
    {
        CurrentHealth = remainingHealth;
        BattlesWon++;
    }
}
