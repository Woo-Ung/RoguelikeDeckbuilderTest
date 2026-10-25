
using UnityEngine;
using UnityEngine.UI;

public class CardSelectionManager : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;

    private CardView selectedCard;

    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradeSelectedCard);

        UpdateUpgradeButton();
    }

    public void SelectCard(CardView card)
    {
        // 이전에 선택된 카드의 강조 해제
        if (selectedCard != null)
        {
            selectedCard.SetSelected(false);
        }

        // 새로운 카드 선택
        selectedCard = card;
        selectedCard.SetSelected(true);

        UpdateUpgradeButton();
    }

    public void ClearSelection(CardView card)
    {
        if (selectedCard != card)
            return;

        selectedCard.SetSelected(false);
        selectedCard = null;

        UpdateUpgradeButton();
    }

    private void UpdateUpgradeButton()
    {
        upgradeButton.interactable = selectedCard != null;
    }

    private void UpgradeSelectedCard()
    {
        if (selectedCard == null)
            return;

        selectedCard.UpgradeCard();
    }

    private void OnDestroy()
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveListener(UpgradeSelectedCard);
        }
    }
}
