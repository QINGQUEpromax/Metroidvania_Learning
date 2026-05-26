using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAir : EntityState
{
    public PlayerInAir(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();
        if(player.moveInput.x != 0)
            player.SetVelocity(player.moveInput.x * player.moveSpeed * player.inAirSpeed, rb.velocity.y);

        if (input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpAttackState);
        }
    }
}
