using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Combat_System : MonoBehaviour
{
    private Character character => GetComponent<Character>();
    private Character_Health health => GetComponent<Character_Health>();
    private Stats_System stats => GetComponent<Stats_System>();
    public float damage = 10;

    [Header("Ä¿±ê¼ì²â")]
    public Transform attackCenter;
    public float attackRadius;
    public LayerMask attackTarget;

    [Header("×´Ì¬Ó°ÏìÂß¼­")]
    [SerializeField] private float defaultDuration = 3;
    [SerializeField] private float chillslowMultipier = .5f;


    public bool attackIsEvaded {  get; private set; }
    
    protected virtual void Update()
    {
       
    }

        

    public void PerformAttack()
    {
        foreach(var target in GetTargetCollider())
        {
            IDamage damagable = target.GetComponent<IDamage>();
            if (damagable == null)
                continue;

            if (target.gameObject.GetComponent<Character_Health>() != null)
            {   
                attackIsEvaded = target.gameObject.GetComponent<Character_Health>().AttackEvaded();
            }
            else
            {
                attackIsEvaded = false;
            }

            if (attackIsEvaded)
                continue;

            float elementDamage = stats.GetElementDamage(out ElementType element);
            float damage = stats.GetPhysicalDamage(out bool isCrit);
            damagable?.TakeDamage(damage, elementDamage,element, health.CalculateDuration(damage), transform);

            if(element != ElementType.None)
            {
                ApplyStatusEffect(target.transform, element);
            }

            if(target.gameObject.GetComponent<Character_VFX>() != null)
            {
            target.gameObject.GetComponent<Character_VFX>().UpdateHitColor(element);
            target.gameObject.GetComponent<Character_VFX>().CreateHitVfx(isCrit);
            }
            
        }
    }

    public void ApplyStatusEffect(Transform target,ElementType element)
    {
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

        if (statusHandler == null) 
            return;

        if (element == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice))
        {
            statusHandler.ApplyChilledEffect(defaultDuration, chillslowMultipier);
        }
    }
    
    protected Collider2D[] GetTargetCollider()
    {
        return Physics2D.OverlapCircleAll(attackCenter.position, attackRadius, attackTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCenter.position, attackRadius);
    }

    //¹¥»÷ÊÂ¼þ
    private void AttackOver()
    {
        character.stateMachine.currentState.attackOver = true;
    }

    
}
