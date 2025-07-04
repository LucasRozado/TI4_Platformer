using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;

public class PlayerCameraSwitch : MonoBehaviour
{
    [SerializeField] private GameObject player; // Reference to the player object

    [Header("Camera References")]
    public CinemachineCamera primaryCamera; // Reference to the primary camera
    public CinemachineCamera currentCamera; // Reference to the current camera
    public CinemachineCamera[] cameras; // Array of Cinemachine virtual cameras
    private Coroutine cameraSwitchCoroutine; // Coroutine for switching cameras

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); // Find the player object by tag if not assigned
        }
        if (primaryCamera == null)
        {
            primaryCamera = BrainStatic.instance.cinemachine; // Get the primary camera from the BrainStatic instance
        }
        if (currentCamera == null)
        {
            currentCamera = primaryCamera; // If no current camera is assigned, use the primary camera
        }
        ClearCameraList();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); // Find the player object by tag if not assigned
        }
        if (currentCamera == null)
        {
            currentCamera = primaryCamera; // If no current camera is assigned, use the primary camera
        }
        primaryCamera = BrainStatic.instance.cinemachine; // Get the primary camera from the BrainStatic instance
        SwitchToCamera(primaryCamera); // Switch to the primary camera at the start
    }
    void Update()
    {
        if (currentCamera == null)
        {
            ClearCameraList(); // Clear the camera list to reset camera state
        }
    }
    public void AddCamera(CinemachineCamera camera)
    {
        // Add a new camera to the array of cameras
        List<CinemachineCamera> cameraList = new List<CinemachineCamera>(cameras);
        cameraList.Add(camera);
        cameras = cameraList.ToArray();
    }
    public void RemoveCamera(CinemachineCamera camera)
    {
        // Remove a camera from the array of cameras
        List<CinemachineCamera> cameraList = new List<CinemachineCamera>(cameras);
        cameraList.Remove(camera);
        cameras = cameraList.ToArray();
    }
    public void SwitchToCamera(CinemachineCamera camera)
    {
        foreach (CinemachineCamera cam in cameras)
        {
            // Disable all cameras except the selected one
            if (cam != primaryCamera)
            {
                cam.gameObject.SetActive(false);
            }
        }
        currentCamera = camera; // Set the current camera to the selected one
        // Enable the selected camera
        camera.gameObject.SetActive(true);
    }
    public void SwitchToPrimaryCamera()
    {
        // Switch to the primary camera
        cameraSwitchCoroutine = StartCoroutine(SwitchToPrimaryCameraCoroutine()); // Start the coroutine to switch to the primary camera
    }
    private IEnumerator SwitchToPrimaryCameraCoroutine()
    {
        SwitchToCamera(primaryCamera); // Switch to the primary camera after a delay
        yield return new WaitForSeconds(0.1f);
        primaryCamera.Follow = player.transform; // Set the player as the follow target for the primary camera
    }
    public void SetPrimaryCamera(Vector3 position, Vector3 rotation)
    {
        // Set the position and rotation of the primary camera
        primaryCamera.Follow = null; // Disable tracking target to set position and rotation
        primaryCamera.transform.position = position;
        primaryCamera.transform.rotation = Quaternion.Euler(rotation);
        DeactivatePrimaryCamera(); // Deactivate the primary camera after setting its position and rotation
    }
    public void SetPrimaryCameraRotation(Vector3 rotation)
    {
        primaryCamera.Follow = null; // Disable tracking target to set position and rotation
        // Set the rotation of the primary camera
        primaryCamera.transform.rotation = Quaternion.Euler(rotation);
        DeactivatePrimaryCamera(); // Deactivate the primary camera after setting its position and rotation
    }
    private void DeactivatePrimaryCamera()
    {
        // Deactivate the primary camera
        primaryCamera.gameObject.SetActive(false);
    }
    public void ClearCameraList()
    {
        // Clear the list of cameras
        cameras = new CinemachineCamera[0];
        currentCamera = primaryCamera; // Reset the current camera
        primaryCamera.GetComponent<BrainStatic>().ActivateCamera(); // Reactivate the primary camera
        primaryCamera.gameObject.SetActive(true); // Reactivate the primary camera
    }
}
