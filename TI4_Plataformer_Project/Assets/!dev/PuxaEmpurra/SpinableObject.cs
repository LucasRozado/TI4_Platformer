using UnityEngine;

public class SpinableObject : Interactable
{
    [SerializeField] float rotationSpeed;
    [SerializeField] GameObject spinObject;
    public override void InteractWith(Player player)
    {
        player.GetState<PlayerState_Spin>().HandleObject(spinObject, rotationSpeed);
        player.SwitchState<PlayerState_Spin>();
    }
}
