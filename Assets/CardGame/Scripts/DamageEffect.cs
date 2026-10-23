using UnityEngine;

[CreateAssetMenu(
    fileName = "NewDamageEffect",
    menuName = "Card Game/Effects/Damage")]
public class DamageEffect : CardEffect
{
    public override void Execute(Player player, Enemy target, int value)
    {
        target.TakeDamage(value);
    }
}
