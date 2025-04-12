using UnityEngine;

public class CPInteraction : Interactable
{
    [SerializeField] CPInfo info;
    bool isActive;
    public override void InteractWith(Player player)
    {
        GameManager.SetSpawnPosition(info.spawnPosition);
        if (!isActive)
        {
            isActive = true;
            CPManager.instance.AddCheckPoint(info);
        }
        else
        {
            UIManager.instance.OpenFastTravel();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckActivation(); //TODO change sprite do include drawing
    }
    public void CheckActivation()
    {
        if (!CPManager.instance.VerifyCheckPoint(info))
        {
            isActive = true;
        }
    }
}
