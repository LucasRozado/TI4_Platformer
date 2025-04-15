using System;
using System.IO;
using UnityEngine;
[Serializable]
public class CollectableData
{
    public int[] collectableScore = new int[4];
    public bool[] jungleCollected = new bool[100];
    public bool[] caveCollected = new bool[100];
    public bool[] canionCollected = new bool[100];
    public bool[] spiritualCollected = new bool[100];

    public void LoadCollectables()
    {
        string path = SaveManager.collectablesPath;
        var content = File.ReadAllText(path);

        var p = JsonUtility.FromJson<CollectableData>(content);
        collectableScore = p.collectableScore;
        jungleCollected = p.jungleCollected;
        caveCollected = p.caveCollected;
        canionCollected = p.canionCollected;
        spiritualCollected = p.spiritualCollected;
    }
}
