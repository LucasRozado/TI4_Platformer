using System.Collections.Generic;
using UnityEngine;

public class CPManager : MonoBehaviour
{
    public static CPManager instance;
    [SerializeField] List<CPInfo> checkpoints;

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
    }

    public void AddCheckPoint(CPInfo info)
    {
        if (!checkpoints.Contains(info))
        {
            checkpoints.Add(info);
        }
    }

    public bool VerifyCheckPoint(CPInfo info)
    {
        if (info == null)
        {
            Debug.Log("Null info");
            return false;
        }
        return checkpoints.Contains(info);
    }

    public List<CPInfo> GetCheckpoints()
    {
        return checkpoints;
    }

}
