using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlide : EntityState
{
    public PlayerWallSlide(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();
        HandleWallSlide();

        if (player.isGrounded) {
            stateMachine.ChangeState(player.idleState);
            player.Flip();
        }

        if (player.isOnWall == false)
            stateMachine.ChangeState(player.idleState);

        if (input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.wallJumpState);
        }

    }
    private void HandleWallSlide()
    {
        if (player.moveInput.y < 0)
            rb.velocity = new Vector2(player.moveInput.x, rb.velocity.y);
        else
            rb.velocity = new Vector2(player.moveInput.x, rb.velocity.y * player.OnWallSpeed);
    }
}
