using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
public enum CollectableType { Jungle, Cave, Canion, Spiritual}
public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance => instance;

    private InputSystem_Actions actions;

    public static Vector3 playerSpawnPosition;    
    private CollectableData collectableData;
    public CPManager checkpointManager;

    [Header("Add all checkpoint info")]
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

        collectableData = new CollectableData();
        //collectableData.Save();
        collectableData.LoadCollectables();

        checkpointManager = new CPManager();
        checkpointManager.Awake();
    }

    public static void SetSpawnPosition(Vector3 newSpawnPosition)
    {
        playerSpawnPosition = newSpawnPosition;
    }
    public InputSystem_Actions Actions => actions;

    public void AddCollectable(CollectableType type, int number)
    {
        collectableData.collectableScore[(int)type]++;

        switch (type)
        {
            case CollectableType.Jungle:
                {
                    collectableData.jungleCollected[number] = true;
                    break;
                }
            case CollectableType.Cave:
                {
                    collectableData.caveCollected[number] = true;
                    break;
                }
            case CollectableType.Canion:
                {
                    collectableData.canionCollected[number] = true;
                    break;
                }
            case CollectableType.Spiritual:
                {
                    collectableData.spiritualCollected[number] = true;
                    break;
                }
        }
    }

    public bool VerifyIfCollected(CollectableType type, int number)
    {
        switch (type)
        {
            case CollectableType.Jungle:
                {
                    return collectableData.jungleCollected[number];
                }
            case CollectableType.Cave:
                {
                    return collectableData.caveCollected[number];
                }
            case CollectableType.Canion:
                {
                    return collectableData.canionCollected[number];
                }
            case CollectableType.Spiritual:
                {
                    return collectableData.spiritualCollected[number];
                }
            default:
                return false;
        }
    }

    public void UpdateCollectables(Collectable[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i].name = "Collectable: " + i.ToString();
            Collectable coll = list[i].GetComponent<Collectable>();
            if (VerifyIfCollected(list[i].GetCollectableType(), list[i].GetNumber()))
            {
                list[i].gameObject.SetActive(false);
                Debug.Log("Collectable " + i + " deactivated");
            }
        }
        Debug.Log("UpdateDone");
    }

    private void OnDestroy()
    {
        SaveManager.Save(SaveType.Collectables, collectableData);
        SaveManager.Save(SaveType.CheckPoints, checkpointManager);
    }
}
