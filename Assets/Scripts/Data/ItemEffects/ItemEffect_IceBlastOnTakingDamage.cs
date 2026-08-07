using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Ice blast", fileName = "Item effect data -Ice blast on taking damage")]
public class ItemEffect_IceBlastOnTakingDamage : ItemEffect_DataSO, ISerializationCallbackReceiver
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask enemyLayer;

    [Space]
    [SerializeField] private float healthPercentTrigger = .25f;
    [SerializeField] private float cooldown;
    [SerializeField, HideInInspector] private float lastTimeUsed;

    [Header("Vfx Objects")]
    [SerializeField] private GameObject iceBlastVfx;
    [SerializeField] private GameObject onHitVfx;

    public override void ExecuteEffect()
    {
        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHealthPercent() <= healthPercentTrigger;

        if (noCooldown && reachedThreshold)
        {
            player.vfx.CreateEffectof(iceBlastVfx, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        }
    }

    private void DamageEnemiesWithIce()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, enemyLayer);

        foreach (var target in enemies)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null) continue;

            Character_Health targetHealth = target.gameObject.GetComponent<Character_Health>();
            float duration = targetHealth != null ? targetHealth.CalculateDuration(iceDamage) : 0;
            damagable.TakeDamage(0, iceDamage, ElementType.Ice, duration, player.transform);

            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);

            player.vfx.CreateEffectof(onHitVfx, target.transform);
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.health.OnTakingDamage += ExecuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.health.OnTakingDamage -= ExecuteEffect;
        player = null;
    }

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        lastTimeUsed = -999;
    }
}
