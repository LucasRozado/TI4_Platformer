using UnityEngine;

public class BossTrigger : Progress
{
    [SerializeField] GameObject[] objectsToStart;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject go in objectsToStart)
            {
                go.SetActive(true);
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
