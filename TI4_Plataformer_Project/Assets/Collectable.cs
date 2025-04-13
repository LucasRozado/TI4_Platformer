using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] CollectableType type;
    int number = 0;
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.AddCollectable(type, number);
        Destroy(gameObject);
    }

    public int SetNumber(int number)
    {
        this.number = number;
        return (int)type;
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
