using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : EnemyState
{
    public EnemyGround(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetection() == true)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
   

