using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card Game/Card Data")]
public class CardData : ScriptableObject
{
    [SerializeField, FormerlySerializedAs("cardName")] private string _cardName;
    [SerializeField, FormerlySerializedAs("cost")] private int _cost;

    [SerializeField, FormerlySerializedAs("effects")] private List<CardEffectEntry> _effects;

    public string CardName => _cardName;
    public int Cost => _cost;
    public IReadOnlyList<CardEffectEntry> Effects => _effects;

    // 효과 중 하나라도 대상이 필요하면 대상 지정이 필요한 카드
    public bool RequiresTarget
    {
        get
        {
            foreach (CardEffectEntry entry in _effects)
            {
                if (entry.Effect != null && entry.Effect.RequiresTarget)
                    return true;
            }

            return false;
        }
    }
}
