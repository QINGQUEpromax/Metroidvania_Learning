using UnityEngine;
using UnityEngine.UI;
using System;

public class Character_Health : MonoBehaviour, IDamagable
{
    public event Action OnTakingDamage;

    private Slider healthBar;
    private Character_VFX vfx;
    private Character character;
    private Stats_System stats;

    [Header("血量健康")]
    [SerializeField] protected float currentHealth;

    [Header("血量回复")]
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private bool canRegenHealth = true;
    public float lastDamageTaken {  get; private set; }
    public bool isDead {  get; private set; }
    protected bool canTakeDamage = true;

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

        SetupHealth();
    }

    private void SetupHealth()
    {
        if (stats == null)
            return;

        currentHealth = stats.GetMaxHealth();
        UpdateHealthBar();

        InvokeRepeating(nameof(RegenHealth), 0, regenInterval);
    }

    //概率闪避攻击
    public bool AttackEvaded()
    {
        if (stats == null)
            return false;
        else
            return UnityEngine.Random.Range(0, 100) < stats.GetEvasion();
    }

    //受伤逻辑
    public virtual void TakeDamage(float damage, float elementDamage, ElementType element, float duration, Transform damageSource)
    {
        if (isDead || canTakeDamage == false)
            return;

        Stats_System attackerStats = damageSource.GetComponent<Stats_System>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float mitigation = stats != null ? stats.GetArmorMitigation(armorReduction) : 0;
        float PhysicalDamage = damage * (1 - mitigation);

        float resistance = stats != null ? stats.GetElementResistance(element) : 0;
        float finalElementDamage = elementDamage * (1 - resistance);

        TakeKnockback(damageSource, PhysicalDamage);
        ReduceHealth(PhysicalDamage + finalElementDamage);

        lastDamageTaken = PhysicalDamage + finalElementDamage;

        OnTakingDamage?.Invoke();
    }

    public void SetCanTakeDamage(bool canTakeDamage) => this.canTakeDamage = canTakeDamage;

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
        if (isDead)
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
    protected virtual void Die()
    {
        isDead = true;
        character.CharacterDeath();
    }

    public float GetHealthPercent() => currentHealth / stats.GetMaxHealth();
    public void SetHealthToPercent(float percent)
    {
        currentHealth = stats.GetMaxHealth() * Mathf.Clamp01(percent);
        UpdateHealthBar();

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
    private Vector2 CalculateKnockback(float damage, Transform damageSource)
    {
        int direction = transform.position.x > damageSource.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? heavyknockbackPower : knockbackPower;
        knockback.x = knockback.x * direction;

        return knockback;
    }

    public float CalculateDuration(float damage)
    {
        if (stats == null)
            return 0;
        else
            return damage / stats.GetMaxHealth() >= heavyDamageThreshold ? heavyknockbackDuration : knockbackDuration;
    }
    private bool IsHeavyDamage(float damage)
    {
        if (stats == null)
            return false;
        else
            return damage / stats.GetMaxHealth() >= heavyDamageThreshold;
    }

}
