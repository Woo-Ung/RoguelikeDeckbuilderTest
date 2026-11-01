
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardSelectionManager : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("upgradeButton")] private Button _upgradeButton;

    private CardView _selectedCard;

    private void Start()
    {
        _upgradeButton.onClick.AddListener(UpgradeSelectedCard);

        UpdateUpgradeButton();
    }

    public void SelectCard(CardView card)
    {
        // 이전에 선택된 카드의 강조 해제
        if (_selectedCard != null)
        {
            _selectedCard.SetSelected(false);
        }

        // 새로운 카드 선택
        _selectedCard = card;
        _selectedCard.SetSelected(true);

        UpdateUpgradeButton();
    }

    public void ClearSelection(CardView card)
    {
        if (_selectedCard != card)
            return;

        _selectedCard.SetSelected(false);
        _selectedCard = null;

        UpdateUpgradeButton();
    }

    private void UpdateUpgradeButton()
    {
        _upgradeButton.interactable = _selectedCard != null;
    }

    private void UpgradeSelectedCard()
    {
        if (_selectedCard == null)
            return;

        _selectedCard.UpgradeCard();
    }

    private void OnDestroy()
    {
        if (_upgradeButton != null)
        {
            _upgradeButton.onClick.RemoveListener(UpgradeSelectedCard);
        }
    }
}
