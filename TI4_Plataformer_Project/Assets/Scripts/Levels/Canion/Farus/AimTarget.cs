using UnityEngine;

public class AimTarget : MonoBehaviour
{
    [SerializeField] Transform[] baseTargets;
    [SerializeField] int baseTargetIndex = 0;
    [SerializeField] float changeTargetTimer = 5f;
    float changeTargetTime = 0f;
    [SerializeField] Transform currentTarget;

    void Start()
    {
        if (baseTargets.Length == 0)
        {
            Debug.LogError("No base targets assigned to AimTarget.");
            return;
        }

        changeTargetTime = changeTargetTimer;
    }
    public void TargetPlayer()
    {
        currentTarget = Player.instance.transform;
    }

    public void TargetBase()
    {
        currentTarget = baseTargets[baseTargetIndex];
    }

    private void Update()
    {
        changeTargetTime -= Time.deltaTime;
        if (changeTargetTime <= 0f)
        {
            changeTargetTime = changeTargetTimer;
            if (baseTargetIndex >= baseTargets.Length - 1)
            {
                baseTargetIndex = 0;
            }
            else
            {
                baseTargetIndex++;
            }
            TargetBase();
        }

        transform.position = currentTarget.position;
    }
}
