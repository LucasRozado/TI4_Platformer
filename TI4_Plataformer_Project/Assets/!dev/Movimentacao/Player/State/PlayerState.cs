using System.Collections.Generic;
using UnityEngine;

public abstract partial class PlayerState : ScriptableObject
{
    protected Player player;
    public void Init(Player player)
    {
        this.player = player;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
}
