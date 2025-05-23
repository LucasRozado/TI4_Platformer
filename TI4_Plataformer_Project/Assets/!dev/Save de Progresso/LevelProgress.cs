using UnityEngine;
using System.IO;
using System;

[CreateAssetMenu(fileName = "levelProgress", menuName = "Scriptable Objects/Saves/LevelProgress")]
[Serializable]
public class LevelProgress : ScriptableObject
{
    public bool[] levelProgress;
    public string[] elementName;
    [SerializeField] private string fileBaseName = "progress";

    private void Awake()
    {
        Load();
    }

    public string SaveProgress()
    {
        string content = JsonUtility.ToJson(this, true);
        string path = Application.persistentDataPath + "/" + fileBaseName + ".json";
        File.WriteAllText(path, content);
        Debug.Log(fileBaseName + " saved");
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
            content = SaveProgress();
        }
        LevelProgress p = JsonUtility.FromJson<LevelProgress>(content);
        levelProgress = p.levelProgress;
    }

    public void Activate(int i)
    {
        levelProgress[i] = true;
    }
}
