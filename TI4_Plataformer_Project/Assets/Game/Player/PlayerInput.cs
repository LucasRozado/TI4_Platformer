using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput
{
    private InputSystem_Actions.PlayerActions actions;

    public PlayerInput(InputSystem_Actions actions)
    {
        this.actions = actions.Player;
        this.actions.Enable();

        InitializeHandlers();
    }
    private void InitializeHandlers()
    {
        Movement = new(actions.Move);
        Jump = new(actions.Jump);
        Interact = new(actions.Interact);
    }

    public InputHandler<Vector2> Movement;
    public InputHandler<float> Jump;
    public InputHandler<float> Interact;

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
