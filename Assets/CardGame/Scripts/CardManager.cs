
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CardManager : MonoBehaviour
{
    [Header("Starting Deck")]
    [SerializeField, FormerlySerializedAs("startingCards")] private CardData[] _startingCards;
    [SerializeField, FormerlySerializedAs("turnManager")] private TurnManager _turnManager;
    [SerializeField, FormerlySerializedAs("cardsPerTurn")] private int _cardsPerTurn = 5;

    private readonly List<CardInstance> _deck = new();
    private readonly List<CardInstance> _hand = new();
    private readonly List<CardInstance> _discardPile = new();

    public int DeckCount => _deck.Count;
    public int HandCount => _hand.Count;
    public int DiscardCount => _discardPile.Count;

    public event Action<CardInstance> OnCardDrawn;

    public event Action<CardInstance> OnCardPlayed;
    public event Action<CardInstance> OnCardDiscarded;
    public event Action OnCardCountChanged;

    private void Awake()
    {
        InitializeDeck();
    }

    private void OnEnable()
    {
        if (_turnManager == null)
            return;

        _turnManager.OnPlayerTurnStarted += DrawTurnCards;
        _turnManager.OnPlayerTurnEnded += DiscardHand;
    }

    private void OnDisable()
    {
        if (_turnManager == null)
            return;

        _turnManager.OnPlayerTurnStarted -= DrawTurnCards;
        _turnManager.OnPlayerTurnEnded -= DiscardHand;
    }

    private void DrawTurnCards()
    {
        DrawCards(_cardsPerTurn);
    }



    public void DiscardHand()
    {
        CardInstance[] cards = _hand.ToArray();

        foreach (CardInstance card in cards)
        {
            if (!_hand.Remove(card))
                continue;

            _discardPile.Add(card);

            OnCardDiscarded?.Invoke(card);
        }

        OnCardCountChanged?.Invoke();

        Debug.Log(
            $"손패 정리 완료 (손패: {HandCount}, " +
            $"버림 더미: {DiscardCount})");
    }



    public bool PlayCard(CardInstance card)
    {
        if (card == null)
            return false;

        // 손패에 존재하는 카드인지 확인
        if (!_hand.Remove(card))
        {
            Debug.LogWarning("손패에 존재하지 않는 카드입니다.");
            return false;
        }

        // 사용한 카드를 버림 더미로 이동
        _discardPile.Add(card);

        OnCardCountChanged?.Invoke();

        Debug.Log(
                    $"카드 사용: {card.Data.CardName} " +
            $"(손패: {HandCount}, 버림 더미: {DiscardCount})");

        OnCardPlayed?.Invoke(card);

        return true;
    }


    public bool HasCardInHand(CardInstance card)
    {
        return card != null && _hand.Contains(card);
    }

    private void InitializeDeck()
    {
        _deck.Clear();
        _hand.Clear();
        _discardPile.Clear();

        foreach (CardData data in _startingCards)
        {
            if (data == null)
                continue;

            CardInstance instance = new CardInstance(data);
            _deck.Add(instance);
        }

        ShuffleDeck();

        Debug.Log($"덱 생성 완료: {DeckCount}장");
    }


    private void ShuffleDeck()
    {
        for (int i = _deck.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);

            CardInstance temp = _deck[i];
            _deck[i] = _deck[randomIndex];
            _deck[randomIndex] = temp;
        }

        Debug.Log("덱 셔플 완료");
    }


    private void RefillDeck()
    {
        if (_discardPile.Count == 0)
            return;

        _deck.AddRange(_discardPile);

        _discardPile.Clear();

        ShuffleDeck();

        Debug.Log($"버림 더미 재구성 완료: 덱 {DeckCount}장");
    }


    public void DrawCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (!DrawCard())
                break;
        }
    }

    public bool DrawCard()
    {
        // 덱이 비었다면 버림 더미로 다시 구성
        if (_deck.Count == 0)
        {
            RefillDeck();
        }

        // 재구성 후에도 카드가 없다면 드로우 실패
        if (_deck.Count == 0)
        {
            Debug.Log("드로우할 카드가 없습니다.");
            return false;
        }

        int lastIndex = _deck.Count - 1;

        CardInstance card = _deck[lastIndex];

        _deck.RemoveAt(lastIndex);
        _hand.Add(card);

        OnCardCountChanged?.Invoke();
        OnCardDrawn?.Invoke(card);

        Debug.Log(
            $"드로우: {card.Data.CardName} " +
            $"(덱: {DeckCount}, 손패: {HandCount}, " +
            $"버림 더미: {DiscardCount})");

        return true;
    }

}
