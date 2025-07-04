using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CPInteraction : Interactable
{
    [SerializeField] GameObject decal;
    [SerializeField] public Transform spawnPosition;
    [SerializeField] CPInfo info;
    [SerializeField] bool isFirstCheckpoint;
    [SerializeField] private Player player;
    [SerializeField] private float activationTime = 1f;
    [SerializeField] private GameObject cameraTrigger;
    private Coroutine activationCoroutine;
    bool isActive;

    private void Awake()
    {
        if (player == null)
        {
            player = Player.instance;
        }
        cameraTrigger.SetActive(false);
    }
    public override void InteractWith(Player player)
    {
        GameManager.SetSpawnPosition(spawnPosition.position);
        cameraTrigger.SetActive(true);
        if (!isActive)
        {
            GameManager.checkpointManager.AddCheckPoint(info);
            player.GetComponent<PlayerState_Grounded>().FreezePlayerPosition(true);
            player.GetComponent<PlayerAnimations>().PaintingAnimation();
            SetAsActive();
        }
        else
        {
            player.GetComponent<PlayerState_Grounded>().FreezePlayerPosition(true);
            player.GetComponent<PlayerAnimations>().SitDownAnimation();
            UIManager.instance.OpenFastTravel();
        }
    }

    private void SetAsActive()
    {
        isActive = true;

        activationCoroutine = StartCoroutine(ActivateCheckpoint());
    }
    private IEnumerator ActivateCheckpoint()
    {
        yield return new WaitForSeconds(activationTime);
        player.GetComponent<PlayerState_Grounded>().FreezePlayerPosition(false);
        decal.SetActive(true);
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
            SetAsActive();
        }
    }
}
