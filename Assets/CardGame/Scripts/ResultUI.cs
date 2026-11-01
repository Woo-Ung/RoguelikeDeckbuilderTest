
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private Button _newRunButton;
    [SerializeField] private Button _mainMenuButton;

    private void Start()
    {
        _resultText.text =
            RunManager.Instance.IsRunCleared ? "클리어!" : "게임 오버";

        _newRunButton.onClick.AddListener(StartNewRun);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void StartNewRun()
    {
        RunManager.Instance.StartNewRun();

        SceneManager.LoadScene(SceneNames.Battle);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(SceneNames.MainMenu);
    }

    private void OnDestroy()
    {
        if (_newRunButton != null)
        {
            _newRunButton.onClick.RemoveListener(StartNewRun);
        }

        if (_mainMenuButton != null)
        {
            _mainMenuButton.onClick.RemoveListener(GoToMainMenu);
        }
    }
}
