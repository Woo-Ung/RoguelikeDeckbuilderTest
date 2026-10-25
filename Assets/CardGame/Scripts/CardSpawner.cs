
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
        if (cardManager != null)
            cardManager.OnCardDrawn += CreateCardView;
    }

    private void OnDisable()
    {
        if (cardManager != null)
            cardManager.OnCardDrawn -= CreateCardView;
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
            playerEnergy);
    }
}
