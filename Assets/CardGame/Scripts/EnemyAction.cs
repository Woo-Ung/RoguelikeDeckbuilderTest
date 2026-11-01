
using UnityEngine;

// 적이 자기 턴에 수행하는 행동의 베이스
public abstract class EnemyAction : ScriptableObject
{
    public abstract void Execute(
        Enemy self,
        Player target,
        int value);

    // 플레이어에게 보여줄 의도 텍스트
    public abstract string GetIntentText(int value);
}
