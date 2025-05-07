using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] CollectableType type;
    int number = 0;
    private void OnTriggerEnter(Collider other)
    {
        GameManager.collectableManager.AddCollectable(type, number);
        if (other.CompareTag("Player"))
            Destroy(gameObject);
    }

    public void SetNumber(int number)
    {
        this.number = number;
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
