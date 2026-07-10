using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : PlayerInAir
{
    public PlayerFall(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
       
    }
    public override void Update()
    {
        base.Update();
        if (player.isGrounded)
            stateMachine.ChangeState(player.idleState);

        if (player.isOnWall)
            stateMachine.ChangeState(player.wallSlideState);
    }
}


