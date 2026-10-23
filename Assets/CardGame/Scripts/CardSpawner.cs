using System;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private CardView cardPrefab;
    [SerializeField] private Transform cardParent;
    [SerializeField] private CardData[] cardDatas;

    [SerializeField] private Player player;
    [SerializeField] private Enemy enemy;
    [SerializeField] private CardSelectionManager selectionManager;

    private void Start()
    {
        foreach (CardData data in cardDatas)
        {
            CardInstance instance = new CardInstance(data);

            CardView card = Instantiate(cardPrefab, cardParent);

            card.Initialize(instance, player, enemy, selectionManager);        
        }        
    }
}
