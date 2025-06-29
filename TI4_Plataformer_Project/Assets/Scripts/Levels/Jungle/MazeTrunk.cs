using UnityEngine;

public class MazeTrunk : Progress
{
    [SerializeField] ParticleSystem explosion;
    public void OnBossPassage()
    {
        LevelProgress.instance.Activate(intReference);
        Destroy(gameObject);
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }

    private void Start()
    {
        if (LevelProgress.instance.GetProgress(intReference))
        {
            Destroy(gameObject);
        }
    }
}
