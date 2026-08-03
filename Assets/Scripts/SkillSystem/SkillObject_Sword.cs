using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_Sword : SkillObject_Base
{
    protected Skill_SwordThrow swordManager;

    protected Transform player;
    private bool shouldComeback;
    private float comebackSpeed = 20;
    protected float maxAllowedDistance = 25;

    protected Vector2 originDir = new Vector2(-1, -1);

    protected virtual void Update()
    {
        transform.rotation = Quaternion.FromToRotation(originDir, rb.velocity);
        HandleComeback();
    }

    public virtual void SetupSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction;

        this.swordManager = swordManager;

        player = swordManager.transform.root;
        playerStats = swordManager.player.stats;
        damageScaleData = swordManager.damageScaleData;
    }

    public void GetSwordBackToPlayer() => shouldComeback = true;
     
    protected void HandleComeback()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > comebackSpeed)
            GetSwordBackToPlayer();

        if (shouldComeback == false)
            return;

        transform.position = Vector2.MoveTowards(transform.position, player.position, comebackSpeed * Time.deltaTime);

        if (distance < .5f)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        StopSword(other);
        DamageEnemiesInRadius(transform, 1);
    }

    protected void StopSword(Collider2D collision)
    {
        rb.simulated = false;
        transform.parent = collision.transform;
    }
}
