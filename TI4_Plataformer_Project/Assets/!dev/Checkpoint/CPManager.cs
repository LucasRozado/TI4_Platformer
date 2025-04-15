using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
[Serializable]
public class CPManager : MonoBehaviour
{
    public static CPManager instance;
    public List<CPInfo> availableCheckpoints;
    public List<int> availableIDs;
    private Dictionary<int,  CPInfo> allCheckpoints = new Dictionary<int, CPInfo>();
    [Header("Add all checkpoint info")]
    public List<CPInfo> allCheckpointsToAdd;

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
        DontDestroyOnLoad(gameObject);
        CreateDictionary();
        LoadCheckPoints();
        UpdateList(); //TODO check save files for List<int> of available checkpoints
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
        foreach (int i  in availableIDs)
        {
            if (allCheckpoints.ContainsKey(i))
            {
                availableCheckpoints.Add(allCheckpoints[i] as CPInfo);
            }
        }
    }

    public void CreateDictionary()
    {
        foreach (CPInfo info in allCheckpointsToAdd)
        {
            Debug.Log(info.ID + " " + info.name);
            allCheckpoints.Add(info.ID, info);
        }
    }

    public void OnDestroy()
    {
        SaveManager.Save(SaveType.CheckPoints, this);
        Debug.Log("Saving Checkpoints");
    }

    public void LoadCheckPoints()
    {
        string path = SaveManager.checkpointsPath;
        var content = File.ReadAllText(path);

        var p = JsonUtility.FromJson<CPManager>(content);

        availableIDs = p.availableIDs;
        Debug.Log("Loaded Checkpoins");
    }
}
