using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounter : PlayerState
{
    private Player_Combat combat;
    private bool counterSomebody;
    public PlayerCounter(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();

        

        counterSomebody = combat.CounterPerformed();
        anim.SetBool("counterPerformed", counterSomebody);
        stateTimer = combat.counterDuration;
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, rb.velocity.y);
        if (combat.counterOver)
        {
            stateMachine.ChangeState(player.idleState);

        }

        if (stateTimer < 0 && !counterSomebody )
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
