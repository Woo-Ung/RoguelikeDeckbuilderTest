using UnityEngine;
using System.Collections.Generic;

public class CardInstance
{
    public CardData Data { get; private set;  }
    public int CurrentCost { get; private set;  }

    private List<int> _effectValues = new List<int>();

    public CardInstance(CardData data)
    {
        Data = data;
        CurrentCost = data.Cost;

        foreach (CardEffectEntry entry in data.Effects)
        {
            _effectValues.Add(entry.Value);
        }
    }

    public int GetEffectValue(int index)
    {
        return _effectValues[index];
    }

    public void UpgradeEffect(int index, int amount)
    {
        _effectValues[index] += amount;
    }
}
