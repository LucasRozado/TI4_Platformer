using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance => instance;

    private InputSystem_Actions actions;

    public static Vector3 playerSpawnPosition;

    public static CollectableManager collectableManager;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        actions = new InputSystem_Actions();
        collectableManager = new CollectableManager();
        //collectableManager.SaveCollectables();
        collectableManager.LoadCollectables();
    }

    public static void SetSpawnPosition(Vector3 newSpawnPosition)
    {
        playerSpawnPosition = newSpawnPosition;
    }
    public InputSystem_Actions Actions => actions;
}
