
using UnityEngine;
using UnityEngine.UI;

public class CardDrawController : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private Button drawButton;

    private void Start()
    {
        drawButton.onClick.AddListener(Draw);
    }

    private void Draw()
    {
        if (!battleManager.IsBattleActive ||
            !turnManager.IsPlayerTurn)
        {
            return;
        }

        cardManager.DrawCard();
    }

    private void OnDestroy()
    {
        if (drawButton != null)
            drawButton.onClick.RemoveListener(Draw);
    }
}
