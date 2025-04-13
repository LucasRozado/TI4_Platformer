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
    
    public int[] collectableScore = new int[4];
    public bool[,] hasCollected = new bool[4, 100];


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

        hasCollected[0, 13] = true;
    }

    public static void SetSpawnPosition(Vector3 newSpawnPosition)
    {
        playerSpawnPosition = newSpawnPosition;
    }
    public InputSystem_Actions Actions => actions;

    public void AddCollectable(CollectableType type, int number)
    {
        collectableScore[(int)type]++;
        hasCollected[(int)type, number] = true;
    }

    public bool VerifyIfCollected(CollectableType type, int number)
    {
        return hasCollected[(int)type, number];
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
}
