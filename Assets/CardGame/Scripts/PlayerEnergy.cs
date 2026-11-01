
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("maxEnergy")] private int _maxEnergy = 3;

    public int CurrentEnergy { get; private set; }
    public int MaxEnergy => _maxEnergy;

    public event Action OnEnergyChanged;

    private void Awake()
    {
        CurrentEnergy = _maxEnergy;
    }

    public bool CanUseEnergy(int amount)
    {
        return amount >= 0 && CurrentEnergy >= amount;
    }

    public bool TryUseEnergy(int amount)
    {
        if (!CanUseEnergy(amount))
            return false;

        CurrentEnergy -= amount;

        OnEnergyChanged?.Invoke();

        return true;
    }

    public void RestoreEnergy()
    {
        CurrentEnergy = _maxEnergy;

        OnEnergyChanged?.Invoke();
    }
}
