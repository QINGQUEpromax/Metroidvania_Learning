using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : Character_Health
{
    private Enemy enemy => GetComponent<Enemy>();
    public override void TakeDamage(float damage,float duration,Transform damageSource)
    {
        base.TakeDamage(damage,duration,damageSource);

        if (isDead)
            return;

        if (damageSource.CompareTag("Player"))
        {
            enemy.TryEnterBattleState(damageSource);
        }
    }
}
