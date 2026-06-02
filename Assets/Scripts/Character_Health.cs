using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Health : MonoBehaviour
{
    private Character_VFX vfx;

    [SerializeField] protected float maxHp = 100;
    [SerializeField] protected bool isDead;

    protected virtual void Awake()
    {
        vfx = GetComponent<Character_VFX>();
    }
    public virtual void TakeDamage(float damage, Transform damageSource) 
    {
        if (isDead)
            return;
        vfx?.PlayerOnDamageVfx();
        ReduceHp(damage);
    }
    
   
    protected void ReduceHp(float damage)
    {
        maxHp -= damage;
        if (maxHp < 0)
            Die();
    }
   
  
    private void Die() {
        isDead = true;
        Debug.Log("Entity died!");
    }
    
}
