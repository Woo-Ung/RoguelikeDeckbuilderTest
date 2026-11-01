
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewBlockEffect",
    menuName = "Card Game/Effects/Block")]
public class BlockEffect : CardEffect
{
    public override void Execute(
        Player player,
        Enemy target,
        int value)
    {
        player.Status.AddBlock(value);
    }

    public override string GetDescription(int value)
    {
        return $"방어도를 {value} 얻습니다.";
    }
}
