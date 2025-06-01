
using Unity.Cinemachine;
using UnityEngine;

public class CameraStatic : MonoBehaviour
{
    public static CameraStatic instance;

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
}
