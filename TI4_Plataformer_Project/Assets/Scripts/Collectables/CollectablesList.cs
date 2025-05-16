using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CollectablesList : MonoBehaviour
{
    public static CollectablesList instance;
    [SerializeField] Collectable[] levelCollectables;

    private void Awake()
    {
        if (instance == null)
            { instance = this; }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        NumberCollectables();
        GameManager.collectableManager.UpdateCollectables(levelCollectables);
    }

    public void NumberCollectables()
    {
        int count = 0;
        foreach (Collectable collectable in levelCollectables)
        {
            collectable.SetNumber(count);
            count++;
        }
    }
}
