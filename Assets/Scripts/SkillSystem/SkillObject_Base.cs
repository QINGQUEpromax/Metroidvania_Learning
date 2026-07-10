using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    private Character_Health health;

    [SerializeField] protected LayerMask enmeyLayer;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach(var target in EnemiesAround(t, radius))
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;

            health = target.GetComponent<Character_Health>();
            if (health == null)
                continue;

            damagable.TakeDamage(1, 1, ElementType.None, health.knockbackDuration, transform);
        }
    }

    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, enmeyLayer);
    }

    protected virtual void OnDrawGizmos()
    {
        if(targetCheck == null)
            targetCheck = transform;

        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }
    
}
