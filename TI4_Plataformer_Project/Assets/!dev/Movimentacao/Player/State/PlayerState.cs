using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public abstract partial class PlayerState : MonoBehaviour
{
    protected Player player;

    private Action onEnter;
    private Action onExit;

    private readonly HashSet<Coroutine> coroutines = new();

    private void Awake()
    {
        this.player = GetComponent<Player>();

        onEnter += EnterState;

        onExit += ExitState;
        onExit += StopCoroutines;
    }

    protected void Update()
    {
        Vector3 velocity = CalculateVelocity(player.Movement, player.Gravity, player.Forward);
        Player.ControllerCollision collision = player.Move(velocity);

        HandleCollisionUpdate(collision);
    }

    public void Enter(PlayerState state)
    {
        this.Exit();
        state.Enter();
    }

    public void Enter()
    {
        onEnter();
        this.enabled = true;
    }
    private void Exit()
    {
        onExit();
        this.enabled = false;
    }

    protected void BindInputStart<TValue>(PlayerInput.InputHandler<TValue> input, Action handler) where TValue : struct
    {
        onEnter += () => input.OnStart += handler;
        onExit += () => input.OnStart -= handler;
    }
    protected void BindInputCancel<TValue>(PlayerInput.InputHandler<TValue> input, Action handler) where TValue : struct
    {
        onEnter += () => input.OnCancel += handler;
        onExit += () => input.OnCancel -= handler;
    }
    protected void BindInputUpdate<TValue>(PlayerInput.InputHandler<TValue> input, Action<TValue> handler) where TValue : struct
    {
        onEnter += () => input.OnUpdate += handler;
        onExit += () => input.OnUpdate -= handler;
    }

    protected void HandleCoroutine(IEnumerator coroutineDefinition)
    {
        Coroutine coroutine = StartCoroutine(coroutineDefinition);
        coroutines.Add(coroutine);
    }
    private void StopCoroutines()
    {
        foreach (Coroutine coroutine in coroutines)
        { StopCoroutine(coroutine); }

        coroutines.Clear();
    }

    public abstract void Initialize();
    protected abstract void EnterState();
    protected abstract void ExitState();
    protected abstract Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward);
    protected abstract void HandleCollisionUpdate(Player.ControllerCollision collision);
}
