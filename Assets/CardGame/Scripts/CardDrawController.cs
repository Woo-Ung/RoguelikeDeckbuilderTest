
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardDrawController : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("cardManager")] private CardManager _cardManager;
    [SerializeField, FormerlySerializedAs("turnManager")] private TurnManager _turnManager;
    [SerializeField, FormerlySerializedAs("battleManager")] private BattleManager _battleManager;
    [SerializeField, FormerlySerializedAs("drawButton")] private Button _drawButton;

    private void Start()
    {
        _drawButton.onClick.AddListener(Draw);
    }

    private void Draw()
    {
        if (!_battleManager.IsBattleActive ||
            !_turnManager.IsPlayerTurn)
        {
            return;
        }

        _cardManager.DrawCard();
    }

    private void OnDestroy()
    {
        if (_drawButton != null)
            _drawButton.onClick.RemoveListener(Draw);
    }
}
