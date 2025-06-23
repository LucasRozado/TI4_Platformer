using System;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    [SerializeField] CollectableType type;
    [SerializeField] GameObject fruitVFX;
    [SerializeField] AudioClip fruitSFX;
    [SerializeField] GameObject mushroomVFX;
    [SerializeField] AudioClip mushroomSFX;
    [SerializeField] GameObject flowerVFX;
    [SerializeField] AudioClip flowerSFX;
    [SerializeField] GameObject spiritVFX;
    [SerializeField] AudioClip spiritSFX;
    public int number = 0;
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            GameManager.collectableManager.AddCollectable(type, number);
            switch (type)
            {
                case CollectableType.Jungle:
                    GlobalSound.instance.PlayClip(fruitSFX);
                    Instantiate(fruitVFX, transform.position, transform.rotation); break;
                case CollectableType.Cave:
                    GlobalSound.instance.PlayClip(mushroomSFX);
                    Instantiate(mushroomVFX, transform.position, transform.rotation); break;
                case CollectableType.Canion:
                    GlobalSound.instance.PlayClip(flowerSFX);
                    Instantiate(flowerVFX, transform.position, transform.rotation); break;
                case CollectableType.Spiritual:
                    GlobalSound.instance.PlayClip(spiritSFX);
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
