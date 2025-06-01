using UnityEngine;

public class MazeTrunk : Progress
{
    public void OnBossPassage()
    {
        levelProgress.data.levelProgress[intReference] = true;
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (levelProgress.data.levelProgress[intReference])
        {
            Destroy(gameObject);
        }
    }
}
