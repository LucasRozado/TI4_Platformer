using UnityEngine;
using System.IO;
using System;
public enum CollectableType { Jungle, Cave, Canion, Spiritual }
[Serializable]
public class CollectableManager
{
    public int[] collectableScore = new int[4];
    public bool[] jungleCollected = new bool[100];
    public bool[] caveCollected = new bool[100];
    public bool[] canionCollected = new bool[100];
    public bool[] spiritualCollected = new bool[100];
    private string collectablesPath = Application.persistentDataPath + "/collectables.json";

    public void SaveCollectables()
    {
        var content = JsonUtility.ToJson(this, true);
        string path = collectablesPath;
        File.WriteAllText(path, content);
        Debug.Log("Collectables save");
        Debug.Log(collectablesPath);
    }

    public void LoadCollectables()
    {
        string path = collectablesPath;
        var content = File.ReadAllText(path);

        var p = JsonUtility.FromJson<CollectableManager>(content);
        collectableScore = p.collectableScore;
        jungleCollected = p.jungleCollected;
        caveCollected = p.caveCollected;
        canionCollected = p.canionCollected;
        spiritualCollected = p.spiritualCollected;
    }

    public void AddCollectable(CollectableType type, int number)
    {
        collectableScore[(int)type]++;

        switch (type)
        {
            case CollectableType.Jungle:
                {
                    jungleCollected[number] = true;
                    break;
                }
            case CollectableType.Cave:
                {
                    caveCollected[number] = true;
                    break;
                }
            case CollectableType.Canion:
                {
                    canionCollected[number] = true;
                    break;
                }
            case CollectableType.Spiritual:
                {
                    spiritualCollected[number] = true;
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
                    return jungleCollected[number];
                }
            case CollectableType.Cave:
                {
                    return caveCollected[number];
                }
            case CollectableType.Canion:
                {
                    return canionCollected[number];
                }
            case CollectableType.Spiritual:
                {
                    return spiritualCollected[number];
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
}
