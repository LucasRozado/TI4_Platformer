using UnityEngine;
using System.IO;
using System;
public enum PowerUps { Push, Torch, Climb, Spirit }
[Serializable]
public class PlayerPowerUp
{
    public bool[] hasPowerUp = new bool[4];
    string savePath = Application.persistentDataPath + "/PowerUps.json";

    public bool GetPowerUp(PowerUps type)
    {
        return hasPowerUp[(int)type];
    }
    public void AcquirePowerUp(PowerUps type)
    {
        hasPowerUp[(int)type] = true;
    }

    public string SavePowerUp()
    {
        string content = JsonUtility.ToJson(this, true);
        string path = savePath;
        File.WriteAllText(path, content);
        Debug.Log(savePath + " saved");
        return content;
    }

    public void LoadPowerUp()
    {
        string path = savePath;
        string content;
        try
        {
            content = File.ReadAllText(path);
        }
        catch
        {
            content = SavePowerUp();
        }
        PlayerPowerUp p = JsonUtility.FromJson<PlayerPowerUp>(content);
        hasPowerUp = p.hasPowerUp;
    }
}
