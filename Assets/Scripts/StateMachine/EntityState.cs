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
    protected Stats_System stats;

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
        UpdateAnimationParameters();
    }
    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    protected virtual void UpdateAnimationParameters()
    {
        
    }

    public void SyncAttackSpeed()
    {
        float attackSpeed = stats.offense.attackSpeed.GetValue();
        anim.SetFloat("attackSpeedMultiplier", attackSpeed);
    }
}
