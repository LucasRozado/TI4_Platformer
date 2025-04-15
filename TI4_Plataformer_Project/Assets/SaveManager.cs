using UnityEngine;
using System;
using System.IO;
public enum SaveType { Collectables, CheckPoints, PowerUp}
[Serializable]
public static class SaveManager
{
    public static string collectablesPath = Application.persistentDataPath + "/collectables.json";
    public static string checkpointsPath = Application.persistentDataPath + "/checkpoints.json";
    public static string powerUpPath = Application.persistentDataPath + "/powerups.json";

    public static void Save(SaveType type, object saveClass)
    {
        var content = JsonUtility.ToJson(saveClass, true);
        string path = collectablesPath;

        switch (type)
        {
            case SaveType.Collectables:
                {
                    path = collectablesPath;
                    break;
                }
            case SaveType.CheckPoints:
                {
                    path = checkpointsPath;
                    break;
                }
            case SaveType.PowerUp:
                {
                    path = powerUpPath;
                    break;
                }
        }
        File.WriteAllText(path, content);
    }
}
