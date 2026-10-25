
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewDamageEffect",
    menuName = "Card Game/Effects/Damage")]
public class DamageEffect : CardEffect
{
    public override void Execute(
        Player player,
        Enemy target,
        int value)
    {
        target.TakeDamage(value);
    }

    public override string GetDescription(int value)
    {
        return $"적에게 피해를 {value} 줍니다.";
    }
}
