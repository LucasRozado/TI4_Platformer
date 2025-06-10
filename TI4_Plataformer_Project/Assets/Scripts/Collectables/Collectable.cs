using System;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    [SerializeField] CollectableType type;
    public int number = 0;
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            GameManager.collectableManager.AddCollectable(type, number);
            Destroy(gameObject);
        }
    }

    public void SetNumber(int number)
    {
        this.number = number;
        gameObject.name = "Collectable: " + number.ToString();
    }

    public int GetNumber()
    {
        return number;
    }

    public CollectableType GetCollectableType()
    {
        return type;
    }
}
