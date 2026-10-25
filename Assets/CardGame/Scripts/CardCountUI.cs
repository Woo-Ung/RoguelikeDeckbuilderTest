
using UnityEngine;
using TMPro;

public class CardCountUI : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;

    [SerializeField] private TMP_Text deckCountText;
    [SerializeField] private TMP_Text handCountText;
    [SerializeField] private TMP_Text discardCountText;

    private void OnEnable()
    {
        if (cardManager != null)
        {
            cardManager.OnCardCountChanged += UpdateCardCountUI;
        }
    }

    private void Start()
    {
        UpdateCardCountUI();
    }

    private void OnDisable()
    {
        if (cardManager != null)
        {
            cardManager.OnCardCountChanged -= UpdateCardCountUI;
        }
    }

    private void UpdateCardCountUI()
    {
        if (cardManager == null)
            return;

        deckCountText.text =
            $"덱: {cardManager.DeckCount}";

        handCountText.text =
            $"손패: {cardManager.HandCount}";

        discardCountText.text =
            $"버림 더미: {cardManager.DiscardCount}";
    }
}
