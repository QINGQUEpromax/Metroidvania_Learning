using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : PlayerState
{
    private float originGravitScale;
    private int dashDir;
    public PlayerDash(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Player_SkillManager.instance.dash.OnStartEffect();
        player.vfx.DoImageEchoEffect(player.dashDuration);

        dashDir = player.moveInput.x != 0 ? (int)player.moveInput.x : player.facingDir;
        stateTimer = player.dashDuration;
        originGravitScale = rb.gravityScale;
        rb.gravityScale = 0;

        player.health.SetCanTakeDamage(false);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * dashDir, 0);

        if (stateTimer < 0)
        {
            if (player.isGrounded)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }
        public override void Exit()
    {
        base.Exit();
        Player_SkillManager.instance.dash.OnEndEffect();

        player.health.SetCanTakeDamage(true);
        player.SetVelocity(0, 0);
        rb.gravityScale = originGravitScale;
    }

    private void CancelDash()
    {
        if(player.isOnWall)
        {
            if (player.isGrounded)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.wallSlideState);
        }
    }
}


