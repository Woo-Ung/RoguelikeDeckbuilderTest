using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CardInstance
{
    public CardData Data { get; private set;  }
    public int CurrentCost { get; private set;  }

    private List<int> effectValues = new List<int>();

    public CardInstance(CardData data)
    {
        Data = data;
        CurrentCost = data.cost;

        foreach (CardEffectEntry entry in data.effects)
        {
            effectValues.Add(entry.value);
        }
    }

    public int GetEffectValue(int index)
    {
        return effectValues[index];
    }

    public void UpgradeEffect(int index, int amount)
    {
        effectValues[index] += amount;
    }
}
