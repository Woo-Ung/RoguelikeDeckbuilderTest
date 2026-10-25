
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Starting Deck")]
    [SerializeField] private CardData[] startingCards;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private int cardsPerTurn = 5;

    private readonly List<CardInstance> deck = new();
    private readonly List<CardInstance> hand = new();
    private readonly List<CardInstance> discardPile = new();

    public int DeckCount => deck.Count;
    public int HandCount => hand.Count;
    public int DiscardCount => discardPile.Count;

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
        if (turnManager == null)
            return;

        turnManager.OnPlayerTurnStarted += DrawTurnCards;
        turnManager.OnPlayerTurnEnded += DiscardHand;
    }

    private void OnDisable()
    {
        if (turnManager == null)
            return;

        turnManager.OnPlayerTurnStarted -= DrawTurnCards;
        turnManager.OnPlayerTurnEnded -= DiscardHand;
    }

    private void DrawTurnCards()
    {
        DrawCards(cardsPerTurn);
    }



    public void DiscardHand()
    {
        CardInstance[] cards = hand.ToArray();

        foreach (CardInstance card in cards)
        {
            if (!hand.Remove(card))
                continue;

            discardPile.Add(card);

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
        if (!hand.Remove(card))
        {
            Debug.LogWarning("손패에 존재하지 않는 카드입니다.");
            return false;
        }

        // 사용한 카드를 버림 더미로 이동
        discardPile.Add(card);

        OnCardCountChanged?.Invoke();

        Debug.Log(
                    $"카드 사용: {card.Data.cardName} " +
            $"(손패: {HandCount}, 버림 더미: {DiscardCount})");

        OnCardPlayed?.Invoke(card);

        return true;
    }


    public bool HasCardInHand(CardInstance card)
    {
        return card != null && hand.Contains(card);
    }

    private void InitializeDeck()
    {
        deck.Clear();
        hand.Clear();
        discardPile.Clear();

        foreach (CardData data in startingCards)
        {
            if (data == null)
                continue;

            CardInstance instance = new CardInstance(data);
            deck.Add(instance);
        }

        ShuffleDeck();

        Debug.Log($"덱 생성 완료: {DeckCount}장");
    }


    private void ShuffleDeck()
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);

            CardInstance temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }

        Debug.Log("덱 셔플 완료");
    }


    private void RefillDeck()
    {
        if (discardPile.Count == 0)
            return;

        deck.AddRange(discardPile);

        discardPile.Clear();

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
        if (deck.Count == 0)
        {
            RefillDeck();
        }

        // 재구성 후에도 카드가 없다면 드로우 실패
        if (deck.Count == 0)
        {
            Debug.Log("드로우할 카드가 없습니다.");
            return false;
        }

        int lastIndex = deck.Count - 1;

        CardInstance card = deck[lastIndex];

        deck.RemoveAt(lastIndex);
        hand.Add(card);

        OnCardCountChanged?.Invoke();
        OnCardDrawn?.Invoke(card);

        Debug.Log(
            $"드로우: {card.Data.cardName} " +
            $"(덱: {DeckCount}, 손패: {HandCount}, " +
            $"버림 더미: {DiscardCount})");

        return true;
    }

}
