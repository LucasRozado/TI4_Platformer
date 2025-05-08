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
    public readonly InputHandler<float> Sprint;

    public PlayerInput(InputSystem_Actions actions)
    {
        this.actions = actions.Player;
        this.actions.Enable();

        Movement = new(this.actions.Move);
        Jump = new(this.actions.Jump);
        Sprint = new(this.actions.Sprint);
        Interact = new(this.actions.Interact);
    }

    public class InputHandler<TValue> where TValue : struct
    {
        public Action OnStart;
        public Action OnCancel;
        public Action<TValue> OnUpdate;

        public TValue Value { get; private set; }
        public float LastStart { get; private set; }
        public float LastUpdate { get; private set; }

        public InputHandler(InputAction action)
        {
            action.started += (context) =>
            {
                LastStart = Time.time;

                OnStart?.Invoke();
            };

            action.canceled += (context) => OnCancel?.Invoke();

            void OnUpdate(InputAction.CallbackContext context)
            {
                TValue input = context.ReadValue<TValue>();

                Value = input;
                LastUpdate = Time.time;

                this.OnUpdate?.Invoke(input);
            }
            action.performed += OnUpdate;
            action.canceled += OnUpdate;
        }
    }

    ~PlayerInput() => actions.Disable();
}
