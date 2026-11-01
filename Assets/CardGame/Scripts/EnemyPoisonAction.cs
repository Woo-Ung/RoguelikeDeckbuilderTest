
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewEnemyPoisonAction",
    menuName = "Card Game/Enemy Actions/Poison")]
public class EnemyPoisonAction : EnemyAction
{
    public override void Execute(
        Enemy self,
        Player target,
        int value)
    {
        target.Status.AddPoison(value);
    }

    public override string GetIntentText(int value)
    {
        return $"중독 {value}";
    }
}
