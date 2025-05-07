using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
[Serializable]
public class CPManager
{
    public static CPManager instance;
    List<CPInfo> availableCheckpoints = new List<CPInfo>();
    [SerializeField] List<int> availableIDs = new List<int>();
    private Dictionary<int,  CPInfo> allCheckpoints = new Dictionary<int, CPInfo>();
    private string checkpointsPath = Application.persistentDataPath + "/checkpoints.json";

    public void StartManager()
    {
        CreateDictionary();
        UpdateList(); //TODO check save files for List<int> of available checkpoints
    }

    public void SaveCheckPoints()
    {
        var content = JsonUtility.ToJson(this, true);
        string path = checkpointsPath;
        File.WriteAllText(path, content);
    }
    public void LoadCheckPoints()
    {
        string path = checkpointsPath;
        var content = File.ReadAllText(path);

        var p = JsonUtility.FromJson<CPManager>(content);

        availableIDs = p.availableIDs;
        Debug.Log("Loaded Checkpoins");
    }

    public void AddCheckPoint(CPInfo info)
    {
        if (!availableCheckpoints.Contains(info))
        {
            availableCheckpoints.Add(info);
            availableIDs.Add(info.ID);
        }
    }

    public bool VerifyCheckPoint(CPInfo info)
    {
        if (info == null)
        {
            Debug.Log("Null info");
            return false;
        }
        return availableCheckpoints.Contains(info);
    }

    public List<CPInfo> GetCheckpoints()
    {
        return availableCheckpoints;
    }

    public void UpdateList()
    {
        foreach (int i in availableIDs)
        {
            if (allCheckpoints.ContainsKey(i))
            {
                availableCheckpoints.Add(allCheckpoints[i] as CPInfo);
            }
        }
    }

    public void CreateDictionary()
    {
        foreach (CPInfo info in GameManager.Instance.allCheckpointsToAdd)
        {
            Debug.Log(info.ID + " " + info.name);
            allCheckpoints.Add(info.ID, info);
        }
    }    
}
