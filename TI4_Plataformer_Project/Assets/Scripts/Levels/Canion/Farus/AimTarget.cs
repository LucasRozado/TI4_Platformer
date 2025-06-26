using UnityEngine;

public class AimTarget : MonoBehaviour
{
    [SerializeField] Transform baseTarget;
    [SerializeField] Transform currentTarget;

    public void TargetPlayer()
    {
        currentTarget = Player.instance.transform;
    }

    public void TargetBase()
    {
        currentTarget = baseTarget;
    }

    private void Update()
    {
        transform.position = currentTarget.position;
    }
}
