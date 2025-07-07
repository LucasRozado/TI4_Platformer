using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;


public class ConnectToCameraTrigger : MonoBehaviour
{
    [SerializeField] private CameraSwitch cameraTrigger;
    [SerializeField] private bool lockIntoPlayer = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTrigger.SetTargetCamera(this.GetComponent<CinemachineCamera>());
        if (lockIntoPlayer)
        {
            LockCameraToPlayer();
        }
    }
    private void LockCameraToPlayer()
    {
        this.gameObject.GetComponent<CinemachineCamera>().Follow = Player.instance.transform;
    }
}
