
using System;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private int maxEnergy = 3;

    public int CurrentEnergy { get; private set; }
    public int MaxEnergy => maxEnergy;

    public event Action OnEnergyChanged;

    private void Awake()
    {
        CurrentEnergy = maxEnergy;
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
        CurrentEnergy = maxEnergy;

        OnEnergyChanged?.Invoke();
    }
}
