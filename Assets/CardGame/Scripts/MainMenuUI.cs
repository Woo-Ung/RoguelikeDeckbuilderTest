
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _newGameButton;

    private void Start()
    {
        _newGameButton.onClick.AddListener(StartNewGame);
    }

    private void StartNewGame()
    {
        RunManager.Instance.StartNewRun();

        SceneManager.LoadScene(SceneNames.Battle);
    }

    private void OnDestroy()
    {
        if (_newGameButton != null)
        {
            _newGameButton.onClick.RemoveListener(StartNewGame);
        }
    }
}
