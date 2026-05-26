using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState 
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected float stateTimer;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected InputActions input;

    public bool attackOver;
    public EntityState(Player player, StateMachine stateMachine, string stateName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = stateName;
        anim = player.anim;
        rb = player.rb;
        input = player.inputActions;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        attackOver = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        anim.SetFloat("yVelocity", rb.velocity.y);
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);
    }
    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    private bool CanDash()
    {
        if (player.isOnWall || stateMachine.currentState == player.dashState)
            return false;
        return true;
    }
}
