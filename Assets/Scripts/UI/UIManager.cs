using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] public StartWindow startWindow;
    [SerializeField] private EndGameWindow _endGameWindow;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();

        Time.timeScale = 0;
        startWindow.gameObject.SetActive(true);
        startWindow.NewGameEvent += () =>
        {
            startWindow.gameObject.SetActive(false);
            Time.timeScale = 1;
        };

        startWindow.QuitEvent += () => { ExitHelper.Exit(); };

        _gameManager.WinGame += () =>
        {
            _endGameWindow.gameObject.SetActive(true);
            _endGameWindow.winText.gameObject.SetActive(true);
            Time.timeScale = 0;
        };

        _gameManager.LoseGame += () =>
        {
            _endGameWindow.gameObject.SetActive(true);
            _endGameWindow.loseText.gameObject.SetActive(true);
            Time.timeScale = 0;
        };

        _endGameWindow.GoToMenuEvent += () => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); };
    }
}