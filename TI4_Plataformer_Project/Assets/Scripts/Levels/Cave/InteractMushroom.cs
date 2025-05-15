using UnityEngine;
using UnityEngine.Events;

public class InteractMushroom : Interactable
{
    private bool isLit;

    [SerializeField] private Activated onInteractActivate;

    Material material;
    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    public bool IsLit => isLit;

    public override void InteractWith(Player player)
    {
        Light();
    }

    private void Light()
    {
        isLit = true;
        material.color = Color.white;

        onInteractActivate.Activate();
    }
}
