using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_System : MonoBehaviour
{
    private Character character => GetComponent<Character>();
    private Character_Health health => GetComponent<Character_Health>();
    public float damage = 10;

    public Transform attackCenter;
    public float attackRadius;
    public LayerMask attackTarget;
    
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

            damagable?.TakeDamage(damage,health.CalculateDuration(damage), transform);
            target.gameObject.GetComponent<Character>().CreateHitVfx();

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
