using UnityEngine;
using System;

public class Combat_System : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;
    private Character character => GetComponent<Character>();
    private Stats_System stats => GetComponent<Stats_System>();
    public DamageScaleData basicAttackScale;

    [Header("目标检测")]
    public Transform attackCenter;
    public float attackRadius;
    public LayerMask attackTarget;

    [Header("状态效果逻辑")]
    [SerializeField] private float defaultDuration = 3;
    [SerializeField] private float chillslowMultipier = .5f;
    [SerializeField] private float electrifyChargeBuildUp = .4f;
    [Space]
    [SerializeField] private float fireScale = .8f;
    [SerializeField] private float lightningScale = 2.5f;


    public bool attackIsEvaded { get; private set; }

    protected virtual void Update()
    {

    }



    public void PerformAttack()
    {
        foreach (var target in GetTargetCollider())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
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

            ElementalEffectData effectData = new ElementalEffectData(stats, basicAttackScale);

            float elementDamage = stats.GetElementDamage(out ElementType element);
            float PhysicalDamage = stats.GetPhysicalDamage(out bool isCrit);
            Character_Health targetHealth = target.gameObject.GetComponent<Character_Health>();
            float duration = targetHealth != null ? targetHealth.CalculateDuration(PhysicalDamage) : 0;
            damagable?.TakeDamage(PhysicalDamage, elementDamage, element, duration, transform);

            if (element != ElementType.None && target.GetComponent<Character>() != null)
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(element, effectData);

            if (target.gameObject.GetComponent<Character_VFX>() != null)
            {
                OnDoingPhysicalDamage?.Invoke(PhysicalDamage);
                target.gameObject.GetComponent<Character_VFX>().CreateHitVfx(isCrit,element);
            }

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

    //攻击事件
    private void AttackOver()
    {
        character.stateMachine.currentState.attackOver = true;
    }


}
