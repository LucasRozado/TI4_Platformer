using UnityEngine;

public class FarusMachine : BossMachine
{
    [SerializeField] FarusStare stareState;
    public void PlayerFound()
    {
        if (currentState.GetType() == typeof(FarusPatrol))
        {
            ChangeState(stareState);
        }
    }
}
