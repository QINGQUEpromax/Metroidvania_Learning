using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyIdle(this, stateMachine, "idle");
        moveState = new EnemyMove(this, stateMachine, "move");
        attackState = new EnemyAttack(this, stateMachine, "attack");
        battleState = new EnemyBattle(this, stateMachine, "battle");
        dieState = new EnemyDie(this, stateMachine, "die");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.initialize(idleState);
    }
}
