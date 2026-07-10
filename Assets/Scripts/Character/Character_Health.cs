using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Health : MonoBehaviour , IDamagable
{
    private Slider healthBar;
    private Character_VFX vfx;
    private Character character;
    private Stats_System stats;

    [Header("血量健康")]
    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isDead;

    [Header("血量回复")]
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private bool canRegenHealth = true;

    [Header("受伤击退")]
    [SerializeField] private Vector2 knockbackPower;
    [SerializeField] private Vector2 heavyknockbackPower;//重击
    public float knockbackDuration;
    [SerializeField] protected float heavyknockbackDuration;

    [Header("受到重击")]
    [SerializeField] private float heavyDamageThreshold = 0.3f;

    protected virtual void Awake()
    {
        vfx = GetComponent<Character_VFX>();
        character = GetComponent<Character>();
        healthBar = GetComponentInChildren<Slider>();
        stats = GetComponentInChildren<Stats_System>();

        currentHealth = stats.GetMaxHealth();
        UpdateHealthBar();

        InvokeRepeating(nameof(RegenHealth),0, regenInterval);
    }

    //概率闪避攻击
    public bool AttackEvaded() => Random.Range(0, 100) < stats.GetEvasion();


    //受伤逻辑
    public virtual void TakeDamage(float damage,float elementDamage,ElementType element, float duration,Transform damageSource)
    {
        if (isDead)
            return;

        Stats_System attackerStats = damageSource.GetComponent<Stats_System>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float mitigation = stats.GetArmorMitigation(armorReduction);
        float PhysicalDamage = damage * (1 - mitigation);

        float resistance = stats.GetElementResistance(element);
        float finalElementDamage = elementDamage * (1 - resistance);

        TakeKnockback(damageSource, PhysicalDamage);
        ReduceHealth(PhysicalDamage + finalElementDamage);
    }

    //恢复血量
    public void RegenHealth()
    {
        if (!canRegenHealth)
            return;

        float regenAmount = stats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }

    public void IncreaseHealth(float healAmount)
    {
        if(isDead)
            return;

        float newHealth = currentHealth + healAmount;
        float maxHealth = stats.GetMaxHealth();

        currentHealth = Mathf.Min(newHealth, maxHealth);
        UpdateHealthBar();
    }

    //血量减少
    public void ReduceHealth(float damage)
    {
        vfx?.PlayOnDamageVfx();
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
            Die();
    }
   
    //死亡
    private void Die() {
        isDead = true;
        character.CharacterDeath();
    }
    
    //更新血条
    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;

        healthBar.value = currentHealth / stats.GetMaxHealth();
    }

    //实现击退
    private void TakeKnockback(Transform damageSource, float finalDamage)
    {
        Vector2 knockback = CalculateKnockback(finalDamage, damageSource);
        float duration = CalculateDuration(finalDamage);

        character?.Knockback(knockback, duration);
    }

    //计算击退属性
    private Vector2 CalculateKnockback(float damage,Transform damageSource)
    {
        int direction = transform.position.x > damageSource.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? heavyknockbackPower : knockbackPower;
        knockback.x = knockback.x * direction;

        return knockback;
    }

    public float CalculateDuration(float damage) => damage / stats.GetMaxHealth() >= heavyDamageThreshold ? heavyknockbackDuration : knockbackDuration;
    private bool IsHeavyDamage(float damage) => damage / stats.GetMaxHealth() >= heavyDamageThreshold;

}
