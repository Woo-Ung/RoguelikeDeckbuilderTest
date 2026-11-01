using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Card Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string _enemyName;
    [SerializeField] private int _maxHealth = 50;

    // 순서대로 반복 수행할 행동 패턴
    [SerializeField] private List<EnemyMove> _moves;

    public string EnemyName => _enemyName;
    public int MaxHealth => _maxHealth;
    public IReadOnlyList<EnemyMove> Moves => _moves;
}
