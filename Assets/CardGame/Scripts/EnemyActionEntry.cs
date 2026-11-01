using System;
using UnityEngine;

[Serializable]
public class EnemyActionEntry
{
    [SerializeField] private EnemyAction _action;
    [SerializeField] private int _value;

    public EnemyAction Action => _action;
    public int Value => _value;
}
