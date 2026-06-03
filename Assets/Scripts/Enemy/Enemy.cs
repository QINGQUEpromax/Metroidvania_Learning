using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public EnemyIdle idleState;
    public EnemyMove moveState;
    public EnemyAttack attackState;
    public EnemyBattle battleState;
    public EnemyDie dieState;

    [Header("战斗细节")]
    public float battleMoveSpeed;
    public float attackDistance;
    public float battleDuration;//脱战时间
    public float minRetreatDistance = 1;
    public Vector2 retreatVelocity;

    [Header("移动逻辑")]
    public float idleTime = 2f;
    public float moveSpeed = 2f;

    [Header("探路检测")]
    [SerializeField] private float aheadCheckDistance;//敌人检测前方地面距离
    [SerializeField] private Transform roadAheadCheck;//检测敌人前方是否有路

    public bool haveRoadAhead { get; private set; }//敌人检测前方是否有路

    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;

    [Header("玩家检测")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance;

    public Transform player {  get; private set; }

    //敌人死亡
    public override void CharacterDeath()
    {
        base.CharacterDeath();

        stateMachine.ChangeState(dieState);
    }

    //处理玩家死亡时敌人逻辑
    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    //敌人进入战斗状态
    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
            return;
            this.player = player;
        stateMachine.ChangeState(battleState);
    }


    //敌人获取玩家位置
    public Transform GetPlayerTransform()
    {
        if (player == null)
            player = PlayerDetection().transform;

        return player;
    }

    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector3.right * facingDir, playerCheckDistance, playerLayer | groundLayer);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    //检测前方是否有路
    protected override void DetectIsGrounded()
    {
        base.DetectIsGrounded();

        haveRoadAhead = Physics2D.Raycast(roadAheadCheck.position, Vector2.down, aheadCheckDistance, groundLayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(roadAheadCheck.position, roadAheadCheck.position + Vector3.down * facingDir * aheadCheckDistance);//敌人

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(facingDir * playerCheckDistance, 0,0));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(facingDir * attackDistance, 0,0));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(facingDir * minRetreatDistance, 0, 0));

    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
}
