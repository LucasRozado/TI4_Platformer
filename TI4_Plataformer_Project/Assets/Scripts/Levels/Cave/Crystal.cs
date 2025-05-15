using UnityEngine;
using UnityEngine.Events;

public class Crystal : Interactable
{
    private bool isLit;
    private bool canBeToggled = true;

    [SerializeField] private Crystal[] alsoToggle;

    public UnityEvent turnedOn;

    Material material;
    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    public bool IsOn => isLit;

    private void TurnOn()
    {
        isLit = true;
        material.color = Color.white;

        turnedOn.Invoke();
    }
    private void TurnOff()
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

    public void DisableToggle()
    {
        canBeToggled = false;
    }

    public override void InteractWith(Player player)
    {
        foreach (Crystal crystal in alsoToggle)
        { crystal.ToggleOnOff(); }

        ToggleOnOff();
    }
}
