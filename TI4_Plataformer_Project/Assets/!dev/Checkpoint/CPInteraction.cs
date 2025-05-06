using UnityEngine;

public class CPInteraction : Interactable
{
    [SerializeField] Transform spawnPosition;
    [SerializeField] CPInfo info;
    bool isActive;
    public override void InteractWith(Player player)
    {
        GameManager.SetSpawnPosition(spawnPosition.position);
        if (!isActive)
        {
            isActive = true;
            MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
            mesh.material.color = Color.red;
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
        info.spawnPosition = spawnPosition.position;
        CheckActivation(); //TODO change sprite do include drawing
    }
    public void CheckActivation()
    {
        if (CPManager.instance.VerifyCheckPoint(info))
        {
            isActive = true;    
            MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
            mesh.material.color = Color.red;
        }
    }
}
