using UnityEngine;

public class CPInteraction : Interactable
{
    [SerializeField] Transform spawnPosition;
    [SerializeField] CPInfo info;
    [SerializeField] bool isFirstCheckpoint;
    bool isActive;
    public override void InteractWith(Player player)
    {
        GameManager.SetSpawnPosition(spawnPosition.position);
        if (!isActive)
        {
            isActive = true;
            GameManager.checkpointManager.AddCheckPoint(info);
        }
        else
        {
            UIManager.instance.OpenFastTravel();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isFirstCheckpoint)
        {
            GameManager.SetSpawnPosition(spawnPosition.position);
        }
        info.spawnPosition = spawnPosition.position;
        CheckActivation(); //TODO change game object sprite do include drawing
    }
    public void CheckActivation()
    {
        if (GameManager.checkpointManager.VerifyCheckPoint(info))
        {
            isActive = true;
        }
    }
}
