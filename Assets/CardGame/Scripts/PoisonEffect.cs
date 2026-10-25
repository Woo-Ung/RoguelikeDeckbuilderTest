
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewPoisonEffect",
    menuName = "Card Game/Effects/Poison")]
public class PoisonEffect : CardEffect
{
    public override void Execute(
        Player player,
        Enemy target,
        int value)
    {
        target.AddPoison(value);
    }

    public override string GetDescription(int value)
    {
        return $"적에게 중독을 {value} 부여합니다.";
    }
}
