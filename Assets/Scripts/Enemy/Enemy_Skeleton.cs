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
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.initialize(idleState);
    }
}
