
using UnityEngine;
using UnityEngine.Serialization;

public class CardSpawner : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("cardPrefab")] private CardView _cardPrefab;
    [SerializeField, FormerlySerializedAs("cardParent")] private Transform _cardParent;

    [SerializeField, FormerlySerializedAs("cardManager")] private CardManager _cardManager;
    [SerializeField, FormerlySerializedAs("player")] private Player _player;
    [SerializeField] private TargetingManager _targetingManager;
    [SerializeField, FormerlySerializedAs("selectionManager")] private CardSelectionManager _selectionManager;
    [SerializeField, FormerlySerializedAs("turnManager")] private TurnManager _turnManager;
    [SerializeField, FormerlySerializedAs("battleManager")] private BattleManager _battleManager;
    [SerializeField, FormerlySerializedAs("playerEnergy")] private PlayerEnergy _playerEnergy;

    private void OnEnable()
    {
        if (_cardManager == null)
            return;

        _cardManager.OnCardDrawn += CreateCardView;
        _cardManager.OnCardDiscarded += RemoveCardView;
    }

    private void OnDisable()
    {
        if (_cardManager == null)
            return;

        _cardManager.OnCardDrawn -= CreateCardView;
        _cardManager.OnCardDiscarded -= RemoveCardView;
    }

    private void RemoveCardView(CardInstance instance)
    {
        // 현재 손패에 생성된 카드 UI 검색
        CardView[] cards =
            _cardParent.GetComponentsInChildren<CardView>();

        foreach (CardView card in cards)
        {
            if (card.CardInstance != instance)
                continue;

            _selectionManager.ClearSelection(card);
            Destroy(card.gameObject);
            break;
        }
    }

    private void CreateCardView(CardInstance instance)
    {
        CardView card = Instantiate(
            _cardPrefab,
            _cardParent
        );

        card.Initialize(
            instance,
            _player,
            _targetingManager,
            _selectionManager,
            _turnManager,
            _battleManager,
            _playerEnergy,
            _cardManager);
    }

}
