using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card Game/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public int cost;

    public List<CardEffectEntry> effects;
}
