using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_System : MonoBehaviour
{
    public float damage = 10;

    public Transform attackCenter;
    public float attackRadius;
    public LayerMask attackTarget;


    private void Update()
    {
        
    }

    public void PerformAttack()
    {
        foreach(var target in GetTargetCollider())
        {
            Character_Health targetHealth = target.GetComponent<Character_Health>();
            targetHealth?.TakeDamage(damage,transform);
        }
    }
    private Collider2D[] GetTargetCollider()
    {
        return Physics2D.OverlapCircleAll(attackCenter.position, attackRadius, attackTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCenter.position, attackRadius);
    }
}
