
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("playerEnergy")] private PlayerEnergy _playerEnergy;
    [SerializeField, FormerlySerializedAs("energyText")] private TMP_Text _energyText;

    private void OnEnable()
    {
        if (_playerEnergy != null)
            _playerEnergy.OnEnergyChanged += UpdateEnergyUI;
    }

    private void Start()
    {
        UpdateEnergyUI();
    }

    private void OnDisable()
    {
        if (_playerEnergy != null)
            _playerEnergy.OnEnergyChanged -= UpdateEnergyUI;
    }

    private void UpdateEnergyUI()
    {
        if (_playerEnergy == null || _energyText == null)
            return;

        _energyText.text =
            $"{_playerEnergy.CurrentEnergy} / {_playerEnergy.MaxEnergy}";
    }
}
