using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private Button cardButton;
    [SerializeField] private Button selectionButton;

    private CardSelectionManager selectionManager;
    private CardInstance cardInstance;
    private Player player;
    private Enemy target;

    public void Initialize(CardInstance instance, Player player, Enemy enemy)
    {
        cardInstance = instance;
        this.player = player;
        target = enemy;

        cardNameText.text = cardInstance.Data.cardName;
        costText.text = cardInstance.CurrentCost.ToString();

        UpdateEffectText();

        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(UseCard);
    }

    public void UpgradeCard()
    {
        if(cardInstance == null)
        {
            return;
        }
        if(cardInstance.Data.effects.Count == 0)
        {
            return;
        }

        cardInstance.UpgradeEffect(0, 3);

        UpdateEffectText();
    }


    private void UpdateEffectText()
    {
        damageText.text = "";

        for (int i = 0; i < cardInstance.Data.effects.Count; i++)
        {
            CardEffectEntry entry = cardInstance.Data.effects[i];

            if(entry.effect == null)
            {
                continue;               
            }

            int value = cardInstance.GetEffectValue(i);

            damageText.text += $"{entry.effect.name}: {value}\n";
        }
    }

    private void UseCard()
    {
        for (int i = 0; i < cardInstance.Data.effects.Count; i++)
        {
            CardEffectEntry entry = cardInstance.Data.effects[i];

            if (entry.effect == null)
            {
                continue;
            }

            int value = cardInstance.GetEffectValue(i);

            entry.effect.Execute(player, target, value);
        }

        Destroy(gameObject);
    }
}
