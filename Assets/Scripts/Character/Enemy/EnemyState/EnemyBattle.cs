using UnityEngine;

public class EnemyBattle : EnemyState
{
    private Transform player;
    private Transform lastTarget;
    private float lastBattleTime;

    public EnemyBattle(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        UpdateBatteTimer();

        if (player == null)
        {
            player = enemy.GetPlayerTransform();
        }

        if (ShouldRetreat())
        {
            rb.velocity = new Vector2((enemy.retreatVelocity.x * enemy.activeSlowMultiplier) * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetection())
        {
            UpdateTargetIfNeeded();
            UpdateBatteTimer();
        }

        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);

        if (WithinAttackRange() && enemy.PlayerDetection())
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            enemy.SetVelocity(DirectionToPlayer() * enemy.GetBattleMoveSpeed(), rb.velocity.y);
        }
    }

    private void UpdateTargetIfNeeded()
    {
        if (enemy.PlayerDetection() == false)
            return;

        Transform newTarget = enemy.PlayerDetection().transform;

        if (newTarget != lastTarget)
        {
            lastTarget = newTarget;
            player = newTarget;
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
