using UnityEngine;
using UnityEngine.Events;

public class Crystal : Interactable
{
    private bool isLit;
    private bool canBeToggled = true;

    [SerializeField] private Crystal[] alsoToggle;

    public UnityEvent onInteractionFinish;

    Material material;
    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    public bool IsOn => isLit;

    public void TurnOn()
    {
        isLit = true;
        material.color = Color.white;
    }
    public void TurnOff()
    {
        isLit = false;
        material.color = Color.gray;
    }
    public void ToggleOnOff()
    {
        if (!canBeToggled) return;

        if (IsOn)
        { TurnOff(); }
        else
        { TurnOn(); }
    }

    public void EnableToggle()
    {
        canBeToggled = true;
    }
    public void DisableToggle()
    {
        canBeToggled = false;
    }

    public override void InteractWith(Player player)
    {
        foreach (Crystal crystal in alsoToggle)
        { crystal.ToggleOnOff(); }

        ToggleOnOff();

        onInteractionFinish.Invoke();
    }
}
