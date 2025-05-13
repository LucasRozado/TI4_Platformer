using UnityEngine;

public class PushableObject : Interactable
{
    [SerializeField] private float size;
    [SerializeField] private float height;
    public override void InteractWith(Player player)
    {
        if (player.GetPowerUp(PowerUps.Push))
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
}
