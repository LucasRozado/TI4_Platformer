using UnityEngine;
using System.IO;
using System;

[Serializable]
public class EndGame
{
    public bool[] levelComplete = new bool[3];
    string savePath = Application.persistentDataPath + "/EndGame.json";

    public void CompleteLevel(CollectableType type)
    {
        levelComplete[(int)type] = true;
    }

    public bool GetLevel(CollectableType type)
    {
        return levelComplete[(int)type];
    }
    public string SaveEndGame()
    {
        string content = JsonUtility.ToJson(this, true);
        string path = savePath;
        File.WriteAllText(path, content);
        return content;
    }

    public void LoadEndGame()
    {
        string path = savePath;
        string content;
        try
        {
            content = File.ReadAllText(path);
        }
        catch
        {
            content = SaveEndGame();
        }
        EndGame p = JsonUtility.FromJson<EndGame>(content);
        this.levelComplete = p.levelComplete;
    }
}
