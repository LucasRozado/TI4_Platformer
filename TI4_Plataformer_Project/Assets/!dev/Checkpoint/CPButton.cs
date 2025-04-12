using UnityEngine;
using UnityEngine.SceneManagement;

public class CPButton : MonoBehaviour
{
    public CPInfo checkpoint;

    public void Travel()
    {
        if (checkpoint.scene != SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(checkpoint.scene);
            FindFirstObjectByType<Player>().transform.position = checkpoint.spawnPosition;
        }
        else
        {
            Player player = FindFirstObjectByType<Player>();
            player.ToggleController(false);
            player.gameObject.transform.position = checkpoint.spawnPosition;
            player.ToggleController(true);
            UIManager.instance.CloseFastTravel();
        }
    }

    private void OnEnable()
    {
        if (!CPManager.instance.VerifyCheckPoint(checkpoint))
        {
            gameObject.SetActive(false);
        }
    }

    public void PrepareTravel()
    {
        UIManager.instance.PrepareFastTravel(this);
    }
}
