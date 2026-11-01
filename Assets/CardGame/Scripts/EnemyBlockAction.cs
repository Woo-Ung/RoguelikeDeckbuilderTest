
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewEnemyBlockAction",
    menuName = "Card Game/Enemy Actions/Block")]
public class EnemyBlockAction : EnemyAction
{
    public override void Execute(
        Enemy self,
        Player target,
        int value)
    {
        self.Status.AddBlock(value);
    }

    public override string GetIntentText(int value)
    {
        return $"방어 {value}";
    }
}
