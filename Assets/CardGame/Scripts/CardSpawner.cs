
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private CardView cardPrefab;
    [SerializeField] private Transform cardParent;

    [SerializeField] private CardManager cardManager;
    [SerializeField] private Player player;
    [SerializeField] private Enemy enemy;
    [SerializeField] private CardSelectionManager selectionManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private PlayerEnergy playerEnergy;

    private void OnEnable()
    {
        if (cardManager == null)
            return;

        cardManager.OnCardDrawn += CreateCardView;
        cardManager.OnCardDiscarded += RemoveCardView;
    }

    private void OnDisable()
    {
        if (cardManager == null)
            return;

        cardManager.OnCardDrawn -= CreateCardView;
        cardManager.OnCardDiscarded -= RemoveCardView;
    }

    private void RemoveCardView(CardInstance instance)
    {
        // 현재 손패에 생성된 카드 UI 검색
        CardView[] cards =
            cardParent.GetComponentsInChildren<CardView>();

        foreach (CardView card in cards)
        {
            if (card.CardInstance != instance)
                continue;

            selectionManager.ClearSelection(card);
            Destroy(card.gameObject);
            break;
        }
    }

    private void CreateCardView(CardInstance instance)
    {
        CardView card = Instantiate(
            cardPrefab,
            cardParent
        );

        card.Initialize(
            instance,
            player,
            enemy,
            selectionManager,
            turnManager,
            battleManager,
            playerEnergy,
            cardManager);
    }

}
