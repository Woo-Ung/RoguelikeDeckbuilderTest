
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Execute(
        Player player,
        Enemy target,
        int value);

    public abstract string GetDescription(int value);
}
