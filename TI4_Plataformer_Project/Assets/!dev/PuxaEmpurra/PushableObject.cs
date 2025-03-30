using UnityEngine;

public class PushableObject : Interactable
{
    [SerializeField] private float size;
    [SerializeField] private float height;
    public override void InteractWith(Player player)
    {
        player.SwitchState<PlayerState_Pushing>();
    }

    public bool CheckCollision(Player player)
    { 
        Vector3 castPosition = transform.position;
        castPosition.y += height;
        Debug.Log(Physics.Raycast(castPosition, player.Forward, size/2));
        return Physics.Raycast(castPosition, player.Forward, size/2);
    }
}
