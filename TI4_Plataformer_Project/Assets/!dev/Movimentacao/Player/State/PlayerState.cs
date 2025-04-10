using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public abstract partial class PlayerState : MonoBehaviour
{
    protected Player player;
    private HashSet<Coroutine> coroutines;

    private void Awake()
    {
        coroutines = new();

        player = GetComponent<Player>();
        player.AddState(this);
    }

    public void Enter(PlayerState state)
    {
        this.Exit();
        state.Enter();
    }
    public void Enter()
    {
        EnterState();
        this.enabled = true;
    }
    private void Exit()
    {
        ExitState();
        StopCoroutines();
        this.enabled = false;
    }

    private void Update()
    {
        Vector3 velocity = CalculateVelocity(player.Movement, player.Gravity, player.Forward);
        player.Move(velocity);
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

    protected virtual void EnterState() { }
    protected virtual void ExitState() { }
    protected abstract Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward);
}
