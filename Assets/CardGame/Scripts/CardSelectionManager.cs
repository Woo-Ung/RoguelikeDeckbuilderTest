using UnityEngine;
using UnityEngine.UI;

public class CardSelectionManager : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;

    private CardView selectedCard;

    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradeSelectedCard);
    }

    public void SelectCard(CardView card)
    {
        selectedCard = card;

        Debug.Log($"선택한 카드: {card.name}");
    }

    public void ClearSelection(CardView card)
    {
        if(selectedCard == card)
        {
            selectedCard = null;
        }
    }

    private void UpgradeSelectedCard()
    {
        if (selectedCard == null)
        {
            Debug.Log("선택된 카드가 없습니다.");
            return;
        }

        selectedCard.UpgradeCard();
    }

    private void OnDestroy()
    {
        if(upgradeButton != null)
        {
            upgradeButton.onClick.RemoveListener(UpgradeSelectedCard);
        }
    }
}
