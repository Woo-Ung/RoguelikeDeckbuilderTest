using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CardEffectEntry
{
    [SerializeField, FormerlySerializedAs("effect")] private CardEffect _effect;
    [SerializeField, FormerlySerializedAs("value")] private int _value;

    public CardEffect Effect => _effect;
    public int Value => _value;
}
