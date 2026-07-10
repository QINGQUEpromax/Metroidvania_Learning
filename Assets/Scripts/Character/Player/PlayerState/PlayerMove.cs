using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerGrounded
{
    public PlayerMove(Player player, StateMachine stateMechine, string stateName) : base(player, stateMechine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();
        if(player.moveInput.x == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.velocity.y);
    }
}
