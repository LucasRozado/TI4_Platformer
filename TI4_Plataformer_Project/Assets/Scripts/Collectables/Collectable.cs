using System;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    [SerializeField] CollectableType type;
    [SerializeField] GameObject fruitVFX;
    [SerializeField] GameObject mushroomVFX;
    [SerializeField] GameObject flowerVFX;
    [SerializeField] GameObject spiritVFX;
    public int number = 0;
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            GameManager.collectableManager.AddCollectable(type, number);
            switch (type)
            {
                case CollectableType.Jungle:
                    Instantiate(fruitVFX, transform.position, transform.rotation); break;
                case CollectableType.Cave:
                    Instantiate(mushroomVFX, transform.position, transform.rotation); break;
                case CollectableType.Canion:
                    Instantiate(flowerVFX, transform.position, transform.rotation); break;
                case CollectableType.Spiritual:
                    Instantiate(spiritVFX, transform.position, transform.rotation); break;
            }
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
