using System;
using UnityEngine;

// 플레이어와 적이 공통으로 사용하는 전투 상태 (방어도, 중독)
public class CombatStatus
{
    public int Block { get; private set; }
    public int Poison { get; private set; }

    public event Action OnStatusChanged;

    public void AddBlock(int amount)
    {
        Block += Mathf.Max(0, amount);

        OnStatusChanged?.Invoke();
    }

    public void ResetBlock()
    {
        if (Block == 0)
            return;

        Block = 0;

        OnStatusChanged?.Invoke();
    }

    // 방어도로 피해를 흡수하고, 흡수하지 못한 피해량을 반환
    public int AbsorbDamage(int damage)
    {
        int absorbed = Mathf.Min(Block, damage);

        if (absorbed > 0)
        {
            Block -= absorbed;

            OnStatusChanged?.Invoke();
        }

        return damage - absorbed;
    }

    public void AddPoison(int amount)
    {
        Poison += Mathf.Max(0, amount);

        OnStatusChanged?.Invoke();
    }

    // 현재 중독 수치를 피해량으로 반환하고 수치를 1 감소
    public int ConsumePoison()
    {
        int damage = Poison;

        if (damage <= 0)
            return 0;

        Poison--;

        OnStatusChanged?.Invoke();

        return damage;
    }
}
