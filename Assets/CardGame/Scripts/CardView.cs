using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
  

    [Header("Selection UI")]
    [SerializeField] private Image cardBackground;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;

    [Header("Card UI")]
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text damageText;

    [Header("Card Availability")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField, Range(0f, 1f)]
    private float disabledAlpha = 0.5f;

    [Header("Buttons")]
    [SerializeField] private Button cardButton;
    [SerializeField] private Button selectButton;

    private BattleManager battleManager;
    private TurnManager turnManager;
    private CardInstance cardInstance;
    private Player player;
    private Enemy target;
    private CardSelectionManager selectionManager;
    private PlayerEnergy playerEnergy;
    private CardManager cardManager;

    public CardInstance CardInstance => cardInstance;


    public void SetSelected(bool isSelected)
    {
        cardBackground.color =
            isSelected ? selectedColor : normalColor;
    }

    // 카드 초기화
    public void Initialize(
        CardInstance instance,
        Player player,
        Enemy enemy,
        CardSelectionManager manager,
        TurnManager turnManager,
        BattleManager battleManager,
        PlayerEnergy energy,
        CardManager cardManager)
    {
        cardInstance = instance;
        this.player = player;
        target = enemy;
        selectionManager = manager;
        this.turnManager = turnManager;
        this.battleManager = battleManager;
        playerEnergy = energy;
        this.cardManager = cardManager;

        cardNameText.text = cardInstance.Data.cardName;
        costText.text = cardInstance.CurrentCost.ToString();

        UpdateEffectText();
        SetSelected(false);

        cardButton.onClick.RemoveListener(UseCard);
        cardButton.onClick.AddListener(UseCard);

        selectButton.onClick.RemoveListener(SelectCard);
        selectButton.onClick.AddListener(SelectCard);

        playerEnergy.OnEnergyChanged -= UpdateCardAvailability;
        playerEnergy.OnEnergyChanged += UpdateCardAvailability;

        battleManager.OnBattleEnded -= UpdateCardAvailability;
        battleManager.OnBattleEnded += UpdateCardAvailability;

        cardManager.OnCardPlayed -= HandleCardPlayed;
        cardManager.OnCardPlayed += HandleCardPlayed;

        UpdateCardAvailability();
    }


    private void HandleCardPlayed(CardInstance playedCard)
    {
        // 다른 카드가 사용되었다면 무시
        if (playedCard != cardInstance)
            return;

        selectionManager.ClearSelection(this);

        Destroy(gameObject);
    }



    private void UpdateCardAvailability()
    {
        if (cardInstance == null ||
            playerEnergy == null ||
            battleManager == null ||
            turnManager == null)
        {
            return;
        }

        bool canUseCard =
            battleManager.IsBattleActive &&
            turnManager.IsPlayerTurn &&
            playerEnergy.CanUseEnergy(
                cardInstance.CurrentCost);

        cardButton.interactable = canUseCard;

        canvasGroup.alpha =
            canUseCard ? 1f : disabledAlpha;
    }


    // 카드 효과 수치 UI 갱신
    private void UpdateEffectText()
    {
        damageText.text = "";

        for (int i = 0; i < cardInstance.Data.effects.Count; i++)
        {
            CardEffectEntry entry = cardInstance.Data.effects[i];

            if (entry.effect == null)
                continue;

            int value = cardInstance.GetEffectValue(i);

            string description = entry.effect.GetDescription(value);

            damageText.text += description + "\n";
        }
    }

    // 카드 사용
    private void UseCard()
    {
        if (turnManager == null ||
            battleManager == null ||
            playerEnergy == null ||
            cardManager == null ||
            !turnManager.IsPlayerTurn ||
            !battleManager.IsBattleActive)
        {
            return;
        }

        // 손패에 존재하는 카드인지 확인
        if (!cardManager.HasCardInHand(cardInstance))
            return;

        // 에너지 소비
        if (!playerEnergy.TryUseEnergy(cardInstance.CurrentCost))
        {
            Debug.Log("에너지가 부족합니다.");
            return;
        }

        // 카드 효과 실행
        for (int i = 0; i < cardInstance.Data.effects.Count; i++)
        {
            if (!battleManager.IsBattleActive)
                break;

            CardEffectEntry entry = cardInstance.Data.effects[i];

            if (entry.effect == null)
                continue;

            int value = cardInstance.GetEffectValue(i);

            entry.effect.Execute(player, target, value);
        }

        // 카드 이동은 CardManager가 담당
        cardManager.PlayCard(cardInstance);
    }


    // 카드 선택
    private void SelectCard()
    {
        selectionManager.SelectCard(this);
    }

    // 선택된 카드 강화
    public void UpgradeCard()
    {
        if (cardInstance == null)
            return;

        if (cardInstance.Data.effects.Count == 0)
            return;

        cardInstance.UpgradeEffect(0, 3);

        UpdateEffectText();
    }


    private void OnDestroy()
    {
        if (playerEnergy != null)
        {
            playerEnergy.OnEnergyChanged -=
                UpdateCardAvailability;
        }

        if (battleManager != null)
        {
            battleManager.OnBattleEnded -=
                UpdateCardAvailability;
        }

        if (cardButton != null)
        {
            cardButton.onClick.RemoveListener(UseCard);
        }

        if (selectButton != null)
        {
            selectButton.onClick.RemoveListener(SelectCard);
        }

        if (cardManager != null)
        {
            cardManager.OnCardPlayed -= HandleCardPlayed;
        }
    }

}
