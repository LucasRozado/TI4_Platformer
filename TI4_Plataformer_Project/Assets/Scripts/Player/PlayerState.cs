using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public abstract partial class PlayerState : MonoBehaviour
{
    protected Player player;

    private Action bindInputs;
    private Action unbindInputs;

    private readonly HashSet<Coroutine> coroutines = new();

    private void Awake()
    {
        this.player = GetComponent<Player>();
    }

    protected void Update()
    {
        Vector3 velocity = CalculateVelocity(player.Movement, player.Gravity, player.Forward);
        Player.ControllerCollision collision = player.Move(velocity);

        HandleCollisionUpdate(collision);
    }

    public void Enter()
    {
        this.enabled = true;
        bindInputs?.Invoke();
        EnterState();
    }
    public void Exit()
    {
        this.enabled = false;
        unbindInputs?.Invoke();
        StopCoroutines();
        ExitState();
    }

    protected void BindInputStart<TValue>(PlayerInput.InputHandler<TValue> input, Action handler) where TValue : struct
    {
        bindInputs += () => input.OnStart += handler;
        unbindInputs += () => input.OnStart -= handler;
    }
    protected void BindInputCancel<TValue>(PlayerInput.InputHandler<TValue> input, Action handler) where TValue : struct
    {
        bindInputs += () => input.OnCancel += handler;
        unbindInputs += () => input.OnCancel -= handler;
    }
    protected void BindInputUpdate<TValue>(PlayerInput.InputHandler<TValue> input, Action<TValue> handler) where TValue : struct
    {
        bindInputs += () => input.OnUpdate += handler;
        unbindInputs += () => input.OnUpdate -= handler;
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

    public void SwitchReality()
    {
        if (player.GetPowerUp(PowerUps.Spirit))
        {
            SpiritualObserver.instance.SwitchReality();
        }
    }

    public abstract void Initialize();
    protected abstract void EnterState();
    protected abstract void ExitState();
    protected abstract Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward);
    protected abstract void HandleCollisionUpdate(Player.ControllerCollision collision);
}
