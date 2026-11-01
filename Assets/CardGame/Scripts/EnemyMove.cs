using System;
using System.Collections.Generic;
using UnityEngine;

// 적이 한 턴에 수행하는 행동 묶음
[Serializable]
public class EnemyMove
{
    [SerializeField] private List<EnemyActionEntry> _actions;

    public IReadOnlyList<EnemyActionEntry> Actions => _actions;

    // 행동들의 의도 텍스트를 " + "로 연결 (예: 공격 3 + 중독 3)
    public string GetIntentText()
    {
        List<string> texts = new List<string>();

        foreach (EnemyActionEntry entry in _actions)
        {
            if (entry.Action == null)
                continue;

            texts.Add(entry.Action.GetIntentText(entry.Value));
        }

        return string.Join(" + ", texts);
    }
}
