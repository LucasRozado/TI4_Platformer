using UnityEngine;

public class MazeTrunk : Progress
{
    public void OnBossPassage()
    {
        levelProgress.Activate(intReference);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (levelProgress.GetProgress(intReference))
        {
            Destroy(gameObject);
        }
    }
}
