using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class PlayerState : ScriptableObject
{
    protected Player player;
    private HashSet<Coroutine> coroutines;


    public void Configure(Player player)
    {
        this.player = player;
        coroutines = new();
    }


    protected void CoroutineUntilLeaveState(IEnumerator coroutineDefinition)
    {
        StartCoroutine(coroutineDefinition);
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

    protected virtual void EnterState() { }
    protected virtual void ExitState() { }
    public abstract Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward);


    private void StartCoroutine(IEnumerator coroutineDefinition)
    {
        Coroutine coroutine = player.StartCoroutine(coroutineDefinition);
        coroutines.Add(coroutine);
    }

    private void StopCoroutines()
    {
        foreach (Coroutine coroutine in coroutines)
        { player.StopCoroutine(coroutine); }

        coroutines.Clear();
    }
}
