using UnityEngine;

public class BrainStatic : MonoBehaviour
{
    public static BrainStatic instance;
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
