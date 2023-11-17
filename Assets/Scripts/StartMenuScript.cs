using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartMenuScript : MonoBehaviour
{
    Button _startButton;
    Button _exitButton;

    private void OnEnable()
    {

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _startButton = root.Q<Button>("Start");
        _startButton.clicked += () => BeginGame();

        _exitButton = root.Q<Button>("Exit");
        _exitButton.clicked += () => ExitGame();
    }

    void BeginGame()
    {
        SceneManager.LoadScene("IslandScene");
    }

    void ExitGame()
    {
        Application.Quit();
    }

}
