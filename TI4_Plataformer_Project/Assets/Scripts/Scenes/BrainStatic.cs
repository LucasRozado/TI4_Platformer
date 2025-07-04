using Unity.Cinemachine;
using UnityEngine;

public class BrainStatic : MonoBehaviour
{
    public static BrainStatic instance;
    public CinemachineCamera cinemachine;
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
        ActivateCamera();
    }
    public void Start()
    {
        cinemachine = GetComponent<CinemachineCamera>();
    }
    public void ActivateCamera()
    {
        cinemachine.gameObject.SetActive(true);
        cinemachine.Follow = Player.instance.transform;
    }
}
