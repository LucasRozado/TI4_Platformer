using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player; // Reference to the player object
    public CinemachineCamera targetCamera; // Reference to the target camera
    [SerializeField] private CinemachineCamera primaryCamera; // Reference to the primary camera
    [SerializeField] private Vector3 primaryCameraLocation; // Forward camera location
    [SerializeField] private Vector3 primaryCameraRotation; // Forward camera rotation

    private Coroutine removeCameraCoroutine; // Coroutine reference for removing the camera
    void Start()
    {
        primaryCamera = BrainStatic.instance.cinemachine; // Get the primary camera from the player
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player object by tag if not assigned
        if (targetCamera == null)
        {
            targetCamera = primaryCamera; // If no target camera is assigned, use the primary camera
        }
    }
    // Trigger events for camera switching
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (targetCamera == null)
            {
                targetCamera = primaryCamera; // If no target camera is assigned, use the primary camera
            }
            if (player == null)
            {
                primaryCamera = player.GetComponent<PlayerCameraSwitch>().primaryCamera; // Get the primary camera from the player
                player = GameObject.FindGameObjectWithTag("Player"); // Find the player object by tag if not assigned
            }
            if (targetCamera != primaryCamera)
            {
                player.GetComponent<PlayerCameraSwitch>().SetPrimaryCamera(primaryCameraLocation, primaryCameraRotation); // Set the primary camera position and rotation
            }
            else
            {
                player.GetComponent<PlayerCameraSwitch>().SwitchToPrimaryCamera(); // Switch to the primary camera
            }
            player.GetComponent<PlayerCameraSwitch>().AddCamera(targetCamera);
            player.GetComponent<PlayerCameraSwitch>().SwitchToCamera(targetCamera);
            if (removeCameraCoroutine != null)
            {
                StopCoroutine(removeCameraCoroutine); // Stop the coroutine if it's already running
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            removeCameraCoroutine = StartCoroutine(RemoveCamera()); // Start the coroutine to remove the camera
        }
    }
    private IEnumerator RemoveCamera()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds before removing the camera
        player.GetComponent<PlayerCameraSwitch>().RemoveCamera(targetCamera); // Remove the target camera from the player
    }
}
