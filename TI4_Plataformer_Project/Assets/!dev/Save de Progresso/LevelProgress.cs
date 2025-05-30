using UnityEngine;
using System.IO;
using System;

[CreateAssetMenu(fileName = "levelProgress", menuName = "Scriptable Objects/Saves/LevelProgress")]
[Serializable]
public class LevelProgress : ScriptableObject
{
    [SerializeField] private string fileBaseName = "progress";
    public ProgressData data;

    private void Awake()
    {
        Load();
    }

    public string SaveProgress()
    {
        string content = JsonUtility.ToJson(this, true);
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
            content = SaveProgress();
        }
        ProgressData p = JsonUtility.FromJson<ProgressData>(content);
        data.levelProgress = p.levelProgress;
        data.elementName = p.elementName;
    }

    public void Activate(int i)
    {
        data.levelProgress[i] = true;
    }
}
