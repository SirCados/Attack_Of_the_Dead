using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public PlayerCharacterController Player;

    private void LateUpdate()
    {
        
    }
    public void ReloadScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    public void LoadMenuScene()
    {
        //string currentScene = SceneManager.GetActiveScene().name;
        //SceneManager.LoadScene(currentScene);
    }
}
