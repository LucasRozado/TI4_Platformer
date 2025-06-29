using UnityEngine;
using UnityEngine.SceneManagement;

public class CPButton : MonoBehaviour
{
    public CPInfo checkpoint;

    public void Travel()
    {
        if (checkpoint.scene != SceneManager.GetActiveScene().buildIndex)
        {
            Player.instance.transform.parent = null;
            DontDestroyOnLoad(Player.instance);
            SceneManager.LoadScene(checkpoint.scene);
            Player.instance.ToggleController(false);
            Player.instance.gameObject.transform.position = checkpoint.spawnPosition;
            Player.instance.ToggleController(true);

        }
        else
        {
            Player.instance.transform.parent = null;
            DontDestroyOnLoad(Player.instance);
            Player.instance.ToggleController(false);
            Player.instance.gameObject.transform.position = checkpoint.spawnPosition;
            Player.instance.ToggleController(true);
            UIManager.instance.fastTravelScreen.CloseFastTravel();
        }
    }

    private void OnEnable()
    {
        if (!GameManager.checkpointManager.VerifyCheckPoint(checkpoint))
        {
            gameObject.SetActive(false);
        }
    }

    public void PrepareTravel()
    {
        UIManager.instance.fastTravelScreen.PrepareFastTravel(this);
    }
}
