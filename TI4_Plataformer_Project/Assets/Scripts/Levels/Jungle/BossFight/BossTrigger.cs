using UnityEngine;

public class BossTrigger : Progress
{
    [SerializeField] GameObject[] objectsToStart;


    private void OnTriggerEnter(Collider other)
    {
        foreach(GameObject go in objectsToStart)
        {
            go.SetActive(true);
        }
    }

    private void OnEnable()
    {
        foreach (GameObject go in objectsToStart)
        {
            go.SetActive(false);
        }
        if (levelProgress.data.levelProgress[intReference])
        {
            Destroy(gameObject);
        }        
    }
}
