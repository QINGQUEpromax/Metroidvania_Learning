using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : PlayerState
{
    private float attackTimer;
    private float lastAttackTime;
    private bool attackQueued;
    private int attackDir;

    private const int startIndex = 0; 
    private int currentIndex = 0;
    private int maxIndex = 2;
    public PlayerAttack(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        if(maxIndex != player.attackForce.Length - 1)
            maxIndex = player.attackForce.Length;
    }
    public override void Enter()
    {
        base.Enter();
        attackQueued = false;
        ResetAttack();
        SyncAttackSpeed();

        attackDir = player.moveInput.x != 0 ? (int)player.moveInput.x : player.facingDir;

        anim.SetInteger("attackIndex",currentIndex);
        GiveAttackForce();
    }
    public override void Update()
    {
        base.Update();

        HandleAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();

        if (stateMachine.currentState.attackOver)
        {
            HandleStateExit();
        }
    }

    public override void Exit()
    {
        base.Exit();
        lastAttackTime = Time.time;
        currentIndex++;
    }

    private void QueueNextAttack()
    {
        if (currentIndex < maxIndex)
            attackQueued = true;
    }

    private void HandleStateExit()
    {

        if (attackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }

    //¸³Óè¹¥»÷ÕÅÁ¦
    private void GiveAttackForce()
    {
        attackTimer = player.attackDuration;
        player.SetVelocity(player.attackForce[currentIndex].x  * attackDir, player.attackForce[currentIndex].y);
        
    }

    private void HandleAttackVelocity()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
            player.SetVelocity(0, rb.velocity.y);
    }

    private void ResetAttack()
    {
        if(Time.time > lastAttackTime + player.resetInterval || currentIndex > maxIndex)
        {
            currentIndex = startIndex;
        }
    }
}
