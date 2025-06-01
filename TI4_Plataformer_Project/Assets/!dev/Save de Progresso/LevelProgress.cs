using UnityEngine;
using System.IO;
using System;

[CreateAssetMenu(fileName = "levelProgress", menuName = "Scriptable Objects/Saves/LevelProgress")]
[Serializable]
public class LevelProgress : ScriptableObject
{
    [SerializeField] private string fileBaseName = "progress";
    public bool[] levelProgress;
    public string[] progressName;
    public ProgressData data;

    private void Awake()
    {        
        Load();
    }

    public string SaveProgress()
    {
        string content = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/" + fileBaseName + ".json";
        File.WriteAllText(path, content);
        return content;
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/" + fileBaseName + ".json";
        string content;
        try
        {
            content = File.ReadAllText(path);
        }
        catch
        {
            data.levelProgress = new bool[levelProgress.Length];    
            content = SaveProgress();
        }
        ProgressData p = JsonUtility.FromJson<ProgressData>(content);
        data.levelProgress = p.levelProgress;
        levelProgress = p.levelProgress;
    }

    public void Activate(int i)
    {
        data.levelProgress[i] = true;
        levelProgress[i] = true;
    }

    public bool GetProgress(int i)
    {
        return false; //TODO arrumar
        //return levelProgress[i] = true;
    }
}
