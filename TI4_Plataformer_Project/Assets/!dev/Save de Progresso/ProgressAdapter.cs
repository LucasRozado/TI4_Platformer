using UnityEngine;

public class ProgressAdapter : ProgressData
{
    public ProgressAdapter(LevelProgress progress)
    {
        this.levelProgress = progress.levelProgress;
    }

    public static void DataToProgress(ProgressData data, LevelProgress levelProgress)
    {
        levelProgress.levelProgress = data.levelProgress;
    }
}
