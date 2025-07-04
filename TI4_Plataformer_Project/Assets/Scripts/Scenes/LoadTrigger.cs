using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTrigger : MonoBehaviour
{
    [SerializeField] int sceneToGo;
    [SerializeField] LoadSceneMode loadSceneMode;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Player.instance.transform.parent = null;
            DontDestroyOnLoad(Player.instance.gameObject);

            StartCoroutine(LoadScene());
        }
    }
    public IEnumerator LoadScene()
    {
        Player.instance.GetComponent<PlayerCameraSwitch>().ClearCameraList(); // Clear the camera list to
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToGo, loadSceneMode);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
