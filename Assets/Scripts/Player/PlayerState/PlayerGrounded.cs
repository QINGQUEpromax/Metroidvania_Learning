using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : PlayerState
{
    public PlayerGrounded(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

   public override void Update()
    {
        base.Update();

        if (input.Player.Jump.WasPerformedThisFrame())
            stateMachine.ChangeState(player.jumpState);

        if(rb.velocity.y < 0 && player.isGrounded == false)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.attackState);
        }
    }
}
