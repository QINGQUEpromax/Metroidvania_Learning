using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : PlayerState
{
    public PlayerWallJump(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(player.wallJumpForce.x * -player.facingDir, player.wallJumpForce.y);

    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.fallState);
        if (player.isOnWall)
            stateMachine.ChangeState(player.wallSlideState);
        if (input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpAttackState);
        }
    }
}
