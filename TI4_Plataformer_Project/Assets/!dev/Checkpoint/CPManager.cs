using System.Collections.Generic;
using UnityEngine;

public class CPManager : MonoBehaviour
{
    public static CPManager instance;
    [SerializeField] List<CPInfo> availableCheckpoints;
    [SerializeField] List<int> availableIDs;
    private Dictionary<int,  CPInfo> allCheckpoints = new Dictionary<int, CPInfo>();
    [Header("Add all checkpoint info")]
    [SerializeField] List<CPInfo> allCheckpointsToAdd;

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
}
