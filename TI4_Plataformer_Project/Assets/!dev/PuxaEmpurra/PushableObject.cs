using UnityEngine;

public class PushableObject : Interactable
{
    [SerializeField] PlayerState_Pushing pushingState;
    public override void InteractWith(Player player)
    {
        RaycastHit hit;
        Vector3 playerCenter = (player.GetInteractChecks(0).position - player.GetInteractChecks(1).position)/2;
        playerCenter += player.GetInteractChecks(1).position;
        Physics.Raycast(playerCenter, player.Forward, out hit, player.InteractDistance, player.CanInteract);


        player.SwitchState<PlayerState_Pushing>();
    }
}
