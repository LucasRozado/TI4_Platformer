using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector2 movement;
    private bool jump;

    private void Update()
    {
        movement = GetMovementInput();
        jump = GetJumpInput();
    }

    public Vector2 Movement => movement;
    public bool Jump => jump;

    private Vector2 GetMovementInput()
    // Retorna se houve movimento baseado nos inputs do frame atual, e a direção calculada.
    // O Y da direção é sempre 0.
    {
        float inputVertical = Input.GetAxisRaw("Vertical");
        float inputHorizontal = Input.GetAxisRaw("Horizontal");

        Vector2 movementInput = new(
            x: inputHorizontal,
            y: inputVertical
        );

        movementInput.Normalize();

        return movementInput;
    }

    private bool GetJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { return true; }
        else
        { return false; }
    }
}