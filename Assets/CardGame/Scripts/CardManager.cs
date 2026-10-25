
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Starting Deck")]
    [SerializeField] private CardData[] startingCards;

    private readonly List<CardInstance> deck = new();
    private readonly List<CardInstance> hand = new();
    private readonly List<CardInstance> discardPile = new();

    public int DeckCount => deck.Count;
    public int HandCount => hand.Count;
    public int DiscardCount => discardPile.Count;

    public event Action<CardInstance> OnCardDrawn;

    private void Awake()
    {
        InitializeDeck();
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

        Debug.Log($"덱 생성 완료: {DeckCount}장");
    }

    public bool DrawCard()
    {
        if (deck.Count == 0)
        {
            Debug.Log("덱에 카드가 없습니다.");
            return false;
        }

        int lastIndex = deck.Count - 1;

        CardInstance card = deck[lastIndex];

        deck.RemoveAt(lastIndex);
        hand.Add(card);

        OnCardDrawn?.Invoke(card);

        Debug.Log(
            $"드로우: {card.Data.cardName} " +
            $"(덱: {DeckCount}, 손패: {HandCount})");

        return true;
    }
}
