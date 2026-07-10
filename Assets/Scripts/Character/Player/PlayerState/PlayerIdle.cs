using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerGrounded
{
    public PlayerIdle(Player player, StateMachine stateMechine, string stateName) : base(player, stateMechine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, rb.velocity.y);
    }
    public override void Update()
    {
        base.Update();
        if(player.moveInput.x != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
