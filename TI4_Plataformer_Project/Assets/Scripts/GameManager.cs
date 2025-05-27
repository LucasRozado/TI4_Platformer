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
    public static PlayerPowerUp powerUp;
    [Header("All Checkpoints to Add")]
    public List<CPInfo> allCheckpointsToAdd;
    [SerializeField] LevelProgress[] levelProgresses;

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

        powerUp = new PlayerPowerUp();
        powerUp.LoadPowerUp();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            powerUp.AcquirePowerUp(PowerUps.Push);
            Debug.Log("Push acquired");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            powerUp.AcquirePowerUp(PowerUps.Torch);
            Debug.Log("Torch acquired");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            powerUp.AcquirePowerUp(PowerUps.Climb);
            Debug.Log("Climb acquired");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            powerUp.AcquirePowerUp(PowerUps.Spirit);
            Debug.Log("Spirit acquired");
        }
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
        foreach (LevelProgress progress in levelProgresses)
        {
            progress.SaveProgress();
        }
        powerUp.SavePowerUp();
    }

    public void ResetToCheckPoint()
    {

    }
}
