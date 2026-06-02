using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    public EnemyAttack(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Update()
    {
        base.Update();

        if (attackOver)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
