using UnityEngine;
using System.IO;

public class LevelProgress : MonoBehaviour
{
    public string fileBaseName = "progress";
    [HideInInspector] public bool[] levelProgress;
    [HideInInspector] public string[] progressName;
    public static LevelProgress instance;

    private void Awake()
    {        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Load();
    }

    public string SaveProgress()
    {
        string content = JsonUtility.ToJson(new ProgressAdapter(this), true);
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
            int size = levelProgress.Length;
            levelProgress = new bool[size];
            content = SaveProgress();
        }
        ProgressData p = JsonUtility.FromJson<ProgressData>(content);
        ProgressAdapter.DataToProgress(p, this);
    }

    public void Activate(int i)
    {
        Debug.Log($"{i} activate");
        levelProgress[i] = true;
    }

    public bool GetProgress(int i)
    {
        Debug.Log($"{i} {levelProgress[i]}");
        return levelProgress[i] = true;
    }
}
