using UnityEngine;

public class PushableObject : Interactable
{
    [SerializeField] private float size;
    [SerializeField] private float height;
    public override void InteractWith(Player player)
    {
        if (GameManager.powerUp.GetPowerUp(PowerUps.Push))
        {
            player.GetState<PlayerState_Pushing>().HandleObject(this);
            player.SwitchState<PlayerState_Pushing>();
        }        
    }

    public bool CheckCollision(Player player)
    { 
        Vector3 castPosition = transform.position;
        castPosition.y += height;
        return Physics.Raycast(castPosition, player.Forward, size/2);
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.powerUp.GetPowerUp(PowerUps.Push))
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0)
            {
                UIManager.instance.ShowText(tutorialText, tutorialDuration);
                timer = tutorialDuration;
            }
        }
    }
}
