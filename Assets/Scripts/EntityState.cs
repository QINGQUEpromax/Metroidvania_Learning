using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState
{
    protected InputActions input;
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected float stateTimer;

    protected Animator anim;
    protected Rigidbody2D rb;

    public bool attackOver;
    public EntityState(StateMachine stateMachine, string stateName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = stateName;
    }
    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        attackOver = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
       
    }
    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }
}
