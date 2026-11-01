
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewEnemyAttackAction",
    menuName = "Card Game/Enemy Actions/Attack")]
public class EnemyAttackAction : EnemyAction
{
    public override void Execute(
        Enemy self,
        Player target,
        int value)
    {
        target.TakeDamage(value);
    }

    public override string GetIntentText(int value)
    {
        return $"공격 {value}";
    }
}
