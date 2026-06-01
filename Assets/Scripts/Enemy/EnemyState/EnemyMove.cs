using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMove : EnemyState
{
    public EnemyMove(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();

        if (!enemy.haveRoadAhead || enemy.isOnWall)
        {
            enemy.Flip();
        }

    }
    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir,rb.velocity.y);
        if (!enemy.haveRoadAhead || enemy.isOnWall)
        {
            enemy.SetVelocity(0, rb.velocity.y);
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
