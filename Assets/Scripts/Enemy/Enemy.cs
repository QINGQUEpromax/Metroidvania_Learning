using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public EnemyIdle idleState;
    public EnemyMove moveState;

    [Header("ÒÆ¶¯Âß¼­")]
    public float idleTime = 2f;
    public float moveSpeed = 2f;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;
}
