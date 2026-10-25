
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewHealEffect",
    menuName = "Card Game/Effects/Heal")]
public class HealEffect : CardEffect
{
    public override void Execute(
        Player player,
        Enemy target,
        int value)
    {
        player.Heal(value);
    }

    public override string GetDescription(int value)
    {
        return $"체력을 {value} 회복합니다.";
    }
}
