using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : PlayerState

{
    public PlayerDie(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        input.Disable();
        rb.simulated = false;
    }
}
