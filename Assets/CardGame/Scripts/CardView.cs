using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{


    [Header("Selection UI")]
    [SerializeField, FormerlySerializedAs("cardBackground")] private Image _cardBackground;
    [SerializeField, FormerlySerializedAs("normalColor")] private Color _normalColor = Color.white;
    [SerializeField, FormerlySerializedAs("selectedColor")] private Color _selectedColor = Color.yellow;
    [SerializeField] private Color _targetingColor = Color.cyan;

    [Header("Card UI")]
    [SerializeField, FormerlySerializedAs("cardNameText")] private TMP_Text _cardNameText;
    [SerializeField, FormerlySerializedAs("costText")] private TMP_Text _costText;
    [SerializeField, FormerlySerializedAs("damageText")] private TMP_Text _damageText;

    [Header("Card Availability")]
    [SerializeField, FormerlySerializedAs("canvasGroup")] private CanvasGroup _canvasGroup;
    [SerializeField, Range(0f, 1f), FormerlySerializedAs("disabledAlpha")]
    private float _disabledAlpha = 0.5f;

    [Header("Buttons")]
    [SerializeField, FormerlySerializedAs("cardButton")] private Button _cardButton;
    [SerializeField, FormerlySerializedAs("selectButton")] private Button _selectButton;

    private BattleManager _battleManager;
    private TurnManager _turnManager;
    private CardInstance _cardInstance;
    private Player _player;
    private TargetingManager _targetingManager;
    private CardSelectionManager _selectionManager;
    private PlayerEnergy _playerEnergy;
    private CardManager _cardManager;

    public CardInstance CardInstance => _cardInstance;


    public void SetSelected(bool isSelected)
    {
        _cardBackground.color =
            isSelected ? _selectedColor : _normalColor;
    }

    // 적 선택을 기다리는 중인지 표시
    public void SetTargeting(bool isTargeting)
    {
        _cardBackground.color =
            isTargeting ? _targetingColor : _normalColor;
    }

    // 카드 초기화
    public void Initialize(
        CardInstance instance,
        Player player,
        TargetingManager targetingManager,
        CardSelectionManager manager,
        TurnManager turnManager,
        BattleManager battleManager,
        PlayerEnergy energy,
        CardManager cardManager)
    {
        _cardInstance = instance;
        _player = player;
        _targetingManager = targetingManager;
        _selectionManager = manager;
        _turnManager = turnManager;
        _battleManager = battleManager;
        _playerEnergy = energy;
        _cardManager = cardManager;

        _cardNameText.text = _cardInstance.Data.CardName;
        _costText.text = _cardInstance.CurrentCost.ToString();

        UpdateEffectText();
        SetSelected(false);

        _cardButton.onClick.RemoveListener(UseCard);
        _cardButton.onClick.AddListener(UseCard);

        _selectButton.onClick.RemoveListener(SelectCard);
        _selectButton.onClick.AddListener(SelectCard);

        _playerEnergy.OnEnergyChanged -= UpdateCardAvailability;
        _playerEnergy.OnEnergyChanged += UpdateCardAvailability;

        _battleManager.OnBattleEnded -= UpdateCardAvailability;
        _battleManager.OnBattleEnded += UpdateCardAvailability;

        _cardManager.OnCardPlayed -= HandleCardPlayed;
        _cardManager.OnCardPlayed += HandleCardPlayed;

        UpdateCardAvailability();
    }


    private void HandleCardPlayed(CardInstance playedCard)
    {
        // 다른 카드가 사용되었다면 무시
        if (playedCard != _cardInstance)
            return;

        _selectionManager.ClearSelection(this);

        Destroy(gameObject);
    }



    private void UpdateCardAvailability()
    {
        if (_cardInstance == null ||
            _playerEnergy == null ||
            _battleManager == null ||
            _turnManager == null)
        {
            return;
        }

        bool canUseCard =
            _battleManager.IsBattleActive &&
            _turnManager.IsPlayerTurn &&
            _playerEnergy.CanUseEnergy(
                _cardInstance.CurrentCost);

        _cardButton.interactable = canUseCard;

        _canvasGroup.alpha =
            canUseCard ? 1f : _disabledAlpha;
    }


    // 카드 효과 수치 UI 갱신
    private void UpdateEffectText()
    {
        _damageText.text = "";

        for (int i = 0; i < _cardInstance.Data.Effects.Count; i++)
        {
            CardEffectEntry entry = _cardInstance.Data.Effects[i];

            if (entry.Effect == null)
                continue;

            int value = _cardInstance.GetEffectValue(i);

            string description = entry.Effect.GetDescription(value);

            _damageText.text += description + "\n";
        }
    }

    // 카드 클릭 : 대상이 필요하면 대상 지정, 필요 없으면 즉시 사용
    private void UseCard()
    {
        if (!CanPlay())
            return;

        if (!_playerEnergy.CanUseEnergy(_cardInstance.CurrentCost))
        {
            Debug.Log("에너지가 부족합니다.");
            return;
        }

        if (_cardInstance.Data.RequiresTarget)
        {
            _targetingManager.ToggleTargeting(this);
            return;
        }

        PlayOn(null);
    }

    // 카드 사용 : 에너지 소비 → 효과 실행 → 버림 더미 이동
    public void PlayOn(Enemy target)
    {
        if (!CanPlay())
            return;

        // 에너지 소비
        if (!_playerEnergy.TryUseEnergy(_cardInstance.CurrentCost))
        {
            Debug.Log("에너지가 부족합니다.");
            return;
        }

        // 카드 효과 실행
        for (int i = 0; i < _cardInstance.Data.Effects.Count; i++)
        {
            if (!_battleManager.IsBattleActive)
                break;

            CardEffectEntry entry = _cardInstance.Data.Effects[i];

            if (entry.Effect == null)
                continue;

            int value = _cardInstance.GetEffectValue(i);

            entry.Effect.Execute(_player, target, value);
        }

        // 카드 이동은 CardManager가 담당
        _cardManager.PlayCard(_cardInstance);
    }

    // 플레이어 턴, 전투 진행 중, 손패에 있는 카드인지 확인
    private bool CanPlay()
    {
        if (_turnManager == null ||
            _battleManager == null ||
            _playerEnergy == null ||
            _cardManager == null ||
            !_turnManager.IsPlayerTurn ||
            !_battleManager.IsBattleActive)
        {
            return false;
        }

        return _cardManager.HasCardInHand(_cardInstance);
    }


    // 카드 선택
    private void SelectCard()
    {
        _selectionManager.SelectCard(this);
    }

    // 선택된 카드 강화
    public void UpgradeCard()
    {
        if (_cardInstance == null)
            return;

        if (_cardInstance.Data.Effects.Count == 0)
            return;

        _cardInstance.UpgradeEffect(0, 3);

        UpdateEffectText();
    }


    private void OnDestroy()
    {
        if (_playerEnergy != null)
        {
            _playerEnergy.OnEnergyChanged -=
                UpdateCardAvailability;
        }

        if (_battleManager != null)
        {
            _battleManager.OnBattleEnded -=
                UpdateCardAvailability;
        }

        if (_cardButton != null)
        {
            _cardButton.onClick.RemoveListener(UseCard);
        }

        if (_selectButton != null)
        {
            _selectButton.onClick.RemoveListener(SelectCard);
        }

        if (_cardManager != null)
        {
            _cardManager.OnCardPlayed -= HandleCardPlayed;
        }

        if (_targetingManager != null)
        {
            _targetingManager.ReleaseCard(this);
        }
    }

}
