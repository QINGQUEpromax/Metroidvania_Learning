using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerInAir
{ 
    public PlayerJump(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {      

    }
    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(rb.velocity.x, player.jumpForce);
    }

    public override void Update()
    {
         base.Update();

        if (rb.velocity.y <= 0 && stateMachine.currentState != player.jumpAttackState)
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
