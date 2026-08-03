using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    private Character_Health health;

    [SerializeField] private GameObject onHitVfx;
    [Space]
    [SerializeField] protected LayerMask enmeyLayer;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;
    protected Stats_System playerStats;
    protected DamageScaleData damageScaleData;
    protected ElementType usedElement;
    protected Transform lastTarget;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach(var target in GetEnemiesAround(t, radius))
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;

            health = target.GetComponent<Character_Health>();
            if (health == null)
                continue;

            ElementalEffectData effectData = new ElementalEffectData(playerStats, damageScaleData);
            float physDamage = playerStats.GetPhysicalDamage(out bool isCrit, damageScaleData.phyiscal);
            float elemDamage = playerStats.GetElementDamage(out ElementType element, damageScaleData.elemental);

            damagable.TakeDamage(physDamage, elemDamage, element, health.CalculateDuration(physDamage), transform);

            if (element != ElementType.None)
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(element, effectData);

            lastTarget = target.transform;
            Instantiate(onHitVfx, target.transform.position, Quaternion.identity);

            usedElement = element;
        }
    }

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach(var enmey in GetEnemiesAround(transform, 10))
        {
            float distance = Vector2.Distance(transform.position, enmey.transform.position);

            if(distance < closestDistance)
            {
                target = enmey.transform;
                closestDistance = distance;
            }
        }
        return target;
    }

    protected Collider2D[] GetEnemiesAround(Transform t, float radius)
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
