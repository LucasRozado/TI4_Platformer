using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneCanvas : MonoBehaviour
{
    [SerializeField] GameObject instructions;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HowToPlayToggle()
    {
        instructions.SetActive(!instructions.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
