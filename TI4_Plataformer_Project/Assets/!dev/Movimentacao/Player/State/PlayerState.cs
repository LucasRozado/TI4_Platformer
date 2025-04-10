using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Player))]
public abstract partial class PlayerState : MonoBehaviour
{
    protected Player player;
    private HashSet<Coroutine> coroutines;

    private void Awake()
    {
        coroutines = new();
    }
    private void Start()
    {
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
    }
    private void Exit()
    {
        ExitState();
        StopCoroutines();
    }

    protected void CoroutineUntilLeaveState(IEnumerator coroutineDefinition)
    {
        HandleCoroutine(coroutineDefinition);
    }

    private void HandleCoroutine(IEnumerator coroutineDefinition)
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
    public abstract Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward);
}
