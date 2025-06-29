using UnityEngine;

public class BossTrigger : Progress
{
    [SerializeField] GameObject[] objectsToStart;
    [SerializeField] AudioClip bossMusic;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject go in objectsToStart)
            {
                go.SetActive(true);
                AudioManager.PlayMusicLoop(bossMusic);
            }
        }        
    }

    private void Start()
    {
        foreach (GameObject go in objectsToStart)
        {
            go.SetActive(false);
        }
        if (LevelProgress.instance.GetProgress(intReference))
        {
            Destroy(gameObject);
        }        
    }
}
