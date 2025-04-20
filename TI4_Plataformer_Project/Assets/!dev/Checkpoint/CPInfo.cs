using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "CheckPointInfo", menuName = "Scriptable Objects/CheckPointInfo")]
public class CPInfo : ScriptableObject
{
    public int ID;
    public Vector3 spawnPosition;
    public int scene;
    public Sprite sceneImage;
}
