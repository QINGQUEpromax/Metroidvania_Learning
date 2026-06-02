using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : EnemyState
{
    private Transform player;
    private float lastBattleTime;

    public EnemyBattle(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if(player == null)
            player = enemy.PlayerDetection().transform;

        if (ShouldRetreat())
        {
            rb.velocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetection())
            UpdateBatteTimer();

        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);

        if (WithinAttackRange() && enemy.PlayerDetection())
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            enemy.SetVelocity(DirectionToPlayer() * enemy.battleMoveSpeed, rb.velocity.y);
        }
    }

    private void UpdateBatteTimer() => lastBattleTime = Time.time;

    private bool BattleTimeIsOver() => Time.time > lastBattleTime + enemy.battleDuration;

    private bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;

    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;
    private float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    private int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }

}
