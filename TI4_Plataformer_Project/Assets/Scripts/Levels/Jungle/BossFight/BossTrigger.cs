using UnityEngine;

public class BossTrigger : Progress
{
    [SerializeField] GameObject[] objectsToStart;
    [SerializeField] AudioClip bossMusic;
    [SerializeField] AudioSource audioSource;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject go in objectsToStart)
            {
                go.SetActive(true);
                audioSource.Stop();
                audioSource.clip = bossMusic;
                audioSource.loop = true;
                audioSource.Play();
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
