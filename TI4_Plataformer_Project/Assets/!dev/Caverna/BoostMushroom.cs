using UnityEngine;

public class BoostMushroom : MonoBehaviour
{
    [SerializeField, Tooltip("In meters")]
    private float defaultHeight = 2f;

    [SerializeField, Tooltip("In seconds")]
    private float defaultTimeToApex = 0.5f;

    public void Boost(Player player)
    {
        PlayerState_Jump state = player.GetState<PlayerState_Jump>();

        float jumpDefaultHeight = state.defaultHeight;
        float jumpDefaultTimeToApex = state.defaultTimeToApex;

        state.defaultHeight = defaultHeight;
        state.defaultTimeToApex = defaultTimeToApex;

        player.SwitchState(state);

        state.defaultHeight = jumpDefaultHeight;
        state.defaultTimeToApex = jumpDefaultTimeToApex;
    }
}
