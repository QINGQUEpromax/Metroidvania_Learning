using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : EnemyState
{
    public EnemyDie(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        anim.enabled = false;

        rb.gravityScale = 5;
        rb.velocity = new Vector2(rb.velocity.x,15);

        enemy.GetComponent<Collider2D>().enabled = false;

        stateMachine.SwitchOffStatemachine();
    }
}
