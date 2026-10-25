
using UnityEngine;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private PlayerEnergy playerEnergy;
    [SerializeField] private TMP_Text energyText;

    private void OnEnable()
    {
        if (playerEnergy != null)
            playerEnergy.OnEnergyChanged += UpdateEnergyUI;
    }

    private void Start()
    {
        UpdateEnergyUI();
    }

    private void OnDisable()
    {
        if (playerEnergy != null)
            playerEnergy.OnEnergyChanged -= UpdateEnergyUI;
    }

    private void UpdateEnergyUI()
    {
        if (playerEnergy == null || energyText == null)
            return;

        energyText.text =
            $"{playerEnergy.CurrentEnergy} / {playerEnergy.MaxEnergy}";
    }
}
