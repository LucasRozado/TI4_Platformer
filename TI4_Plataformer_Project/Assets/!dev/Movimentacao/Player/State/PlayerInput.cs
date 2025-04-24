using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput
{
    private InputSystem_Actions.PlayerActions actions;

    // Handlers
    public readonly InputHandler<Vector2> Movement;
    public readonly InputHandler<float> Jump;
    public readonly InputHandler<float> Interact;

    // Quick access
    public Vector2 Directional => directional;
    private Vector2 directional;

    public PlayerInput(InputSystem_Actions actions)
    {
        this.actions = actions.Player;
        this.actions.Enable();

        Movement = new(this.actions.Move);
        Jump = new(this.actions.Jump);
        Interact = new(this.actions.Interact);

        Movement.OnUpdate += (input) => directional = input;
    }

    public class InputHandler<TValue> where TValue : struct
    {
        public Action OnStart;
        public Action OnCancel;
        public Action<TValue> OnUpdate;

        public InputHandler(InputAction action)
        {
            action.started += (context) => OnStart?.Invoke();
            action.canceled += (context) => OnCancel?.Invoke();

            void OnUpdate(InputAction.CallbackContext context)
            {
                TValue input = context.ReadValue<TValue>();
                this.OnUpdate?.Invoke(input);
            }
            action.performed += OnUpdate;
            action.canceled += OnUpdate;
        }
    }

    ~PlayerInput() => actions.Disable();
}
