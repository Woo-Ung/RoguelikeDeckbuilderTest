using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Execute(Player player, Enemy target, int value);
}
