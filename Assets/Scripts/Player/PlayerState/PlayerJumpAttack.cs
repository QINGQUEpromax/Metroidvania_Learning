using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAttack : PlayerState
{
    private bool touchGround;
    public PlayerJumpAttack(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        touchGround = false;

        player.SetVelocity(player.jumpAttackForce.x * player.facingDir, player.jumpAttackForce.y);
    }
    public override void Update()
    {
        base.Update();
        if (player.isGrounded && touchGround == false)
        {
            touchGround = true;
            anim.SetTrigger("jumpAttackTrigger");
            player.SetVelocity(0, rb.velocity.y);
            
        }
        if (attackOver && player.isGrounded)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
