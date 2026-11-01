
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public class CardCountUI : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("cardManager")] private CardManager _cardManager;

    [SerializeField, FormerlySerializedAs("deckCountText")] private TMP_Text _deckCountText;
    [SerializeField, FormerlySerializedAs("handCountText")] private TMP_Text _handCountText;
    [SerializeField, FormerlySerializedAs("discardCountText")] private TMP_Text _discardCountText;

    private void OnEnable()
    {
        if (_cardManager != null)
        {
            _cardManager.OnCardCountChanged += UpdateCardCountUI;
        }
    }

    private void Start()
    {
        UpdateCardCountUI();
    }

    private void OnDisable()
    {
        if (_cardManager != null)
        {
            _cardManager.OnCardCountChanged -= UpdateCardCountUI;
        }
    }

    private void UpdateCardCountUI()
    {
        if (_cardManager == null)
            return;

        _deckCountText.text =
            $"덱: {_cardManager.DeckCount}";

        _handCountText.text =
            $"손패: {_cardManager.HandCount}";

        _discardCountText.text =
            $"버림 더미: {_cardManager.DiscardCount}";
    }
}
