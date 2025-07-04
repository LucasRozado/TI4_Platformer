using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;


public class ConnectToCameraTrigger : MonoBehaviour
{
    [SerializeField] private CameraSwitch cameraTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTrigger.SetTargetCamera(this.GetComponent<CinemachineCamera>());

    }
}
