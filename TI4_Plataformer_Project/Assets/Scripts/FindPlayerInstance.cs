using UnityEngine;

public class FindPlayerInstance : MonoBehaviour
{
    [SerializeField] private Player player;
    private void Awake()
    {
        player = Player.instance;
        if (player == null)
        {
            Debug.LogError("Player instance not found in the scene.");
        }

    }
    public void PlayerGetUp()
    {
        if (player != null)
        {
            player.GetComponent<PlayerAnimations>().GetUpAnimation();
            player.GetComponent<PlayerCameraSwitch>().ClearCameraList(); // Clear the camera list to reset camera state
        }
        else
        {
            Debug.LogError("Player instance is null, cannot unfreeze position.");
        }
    }
}
