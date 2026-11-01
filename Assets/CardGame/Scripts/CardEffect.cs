
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    // 사용 시 적 대상 지정이 필요한 효과인지 여부
    public virtual bool RequiresTarget => false;

    public abstract void Execute(
        Player player,
        Enemy target,
        int value);

    public abstract string GetDescription(int value);
}
