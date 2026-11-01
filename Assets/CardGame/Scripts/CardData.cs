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
}
