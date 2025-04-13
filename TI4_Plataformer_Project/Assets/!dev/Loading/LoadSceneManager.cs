using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void LoadAssync(int sceneId)
    {
        StartCoroutine(LoadScene(sceneId));
    }

    public IEnumerator LoadScene(int sceneId)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }    
    }
}
