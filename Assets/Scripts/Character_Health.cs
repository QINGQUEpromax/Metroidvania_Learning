using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Health : MonoBehaviour
{
    private Character_VFX vfx;
    private Character character;

    [Header("血量健康")]
    [SerializeField] protected float currentHp;
    [SerializeField] protected float maxHp = 100;
    [SerializeField] protected bool isDead;

    [Header("受伤击退")]
    [SerializeField] private Vector2 knockbackPower;
    [SerializeField] private Vector2 heavyknockbackPower;//重击
    [SerializeField] private float knockbackDuration;
    [SerializeField] private float heavyknockbackDuration;

    [Header("受到重击")]
    [SerializeField] private float heavyDamageThreshold = 0.3f;

    protected virtual void Awake()
    {
        vfx = GetComponent<Character_VFX>();
        character = GetComponent<Character>();

        currentHp = maxHp;
    }

    //受伤逻辑
    public virtual void TakeDamage(float damage, Transform damageSource) 
    {
        if (isDead)
            return;
        Vector2 knockback = CalculateKnockback(damage,damageSource);
        float duration = CalculateDuration(damage);

        character?.Knockback(knockback, duration);
        vfx?.PlayerOnDamageVfx();
        ReduceHp(damage);
    }
    
   
    //血量减少
    protected void ReduceHp(float damage)
    {
        currentHp -= damage;
        if (currentHp < 0)
            Die();
    }
   
    //死亡
    private void Die() {
        isDead = true;
        character.CharacterDeath();
    }
    
    //计算击退属性
    private Vector2 CalculateKnockback(float damage,Transform damageSource)
    {
        int direction = transform.position.x > damageSource.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? heavyknockbackPower : knockbackPower;
        knockback.x = knockback.x * direction;

        return knockback;
    }

    private float CalculateDuration(float damage) => damage / maxHp >= heavyDamageThreshold ? heavyknockbackDuration : knockbackDuration;
    private bool IsHeavyDamage(float damage) => damage / maxHp >= heavyDamageThreshold;
}
