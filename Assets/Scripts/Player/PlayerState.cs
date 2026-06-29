using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState: EntityState 
{
    protected Player player;

   
    public PlayerState(Player player, StateMachine stateMachine, string stateName) :base(stateMachine,stateName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        stats = player.stats;
        input = player.inputActions;
    }

    public override void Update()
    {
        base.Update();
       
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);
    }

    private bool CanDash()
    {
        if (player.isOnWall || stateMachine.currentState == player.dashState)
            return false;
        return true;
    }

    protected override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        anim.SetFloat("yVelocity", rb.velocity.y);
    }
}
