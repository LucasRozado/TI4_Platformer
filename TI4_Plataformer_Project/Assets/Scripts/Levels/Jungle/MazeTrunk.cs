using UnityEngine;

public class MazeTrunk : Progress
{
    public void OnBossPassage()
    {
        LevelProgress.instance.Activate(intReference);
        Destroy(gameObject);
    }

    private void Start()
    {
        if (LevelProgress.instance.GetProgress(intReference))
        {
            Destroy(gameObject);
        }
    }
}
