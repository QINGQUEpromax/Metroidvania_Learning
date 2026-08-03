using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : PlayerState
{
    public PlayerGrounded(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

   public override void Update()
    {
        base.Update();

        if(rb.velocity.y < 0 && player.isGrounded == false)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (input.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpState);


        if (input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.attackState);
        }

        if (input.Player.Counter.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.counterState);
        }

        if (input.Player.RangeAttack.WasPressedThisFrame() && skillManager.swordThrow.CanUseSkill())
        {
            stateMachine.ChangeState(player.swordThrowState);
        }    
    }
}
