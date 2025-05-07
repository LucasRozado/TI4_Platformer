using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance => instance;

    private InputSystem_Actions actions;

    public static Vector3 playerSpawnPosition;

    public static CollectableManager collectableManager;
    public static CPManager checkpointManager;
    [Header("All Checkpoints to Add")]
    public List<CPInfo> allCheckpointsToAdd;

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
        //collectableManager.SaveCollectables(); //Ligar e desligar se for recomecar o arquivo
        collectableManager.LoadCollectables();

        checkpointManager = new CPManager();
        //checkpointManager.SaveCheckPoints();
        checkpointManager.StartManager();
    }

    public static void SetSpawnPosition(Vector3 newSpawnPosition)
    {
        playerSpawnPosition = newSpawnPosition;
    }
    public InputSystem_Actions Actions => actions;

    private void OnDestroy()
    {
        collectableManager.SaveCollectables();
        checkpointManager.SaveCheckPoints();
    }

    public void ResetToCheckPoint()
    {

    }
}
