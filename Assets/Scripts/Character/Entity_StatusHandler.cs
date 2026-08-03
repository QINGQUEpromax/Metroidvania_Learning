using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Character character;
    private Character_VFX vfx;
    private Stats_System stats;
    private Character_Health health;
    private ElementType currentEffect = ElementType.None;


    [Header("ÉÁ»÷Âß¼­")]
    [SerializeField] private GameObject lightningStrikeVfx;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximumCharge = 1;
    private Coroutine shockCo;
    private void Awake()
    {
        health = GetComponent<Character_Health>();
        character = GetComponent<Character>();
        stats = GetComponent<Stats_System>();
        vfx = GetComponent<Character_VFX>();
    }

    public void RemoveAllNegativeEffects()
    {
        StopAllCoroutines();
        currentEffect = ElementType.None;
        vfx.StopAllVfx();
    }

    public void ApplyStatusEffect(ElementType element, ElementalEffectData effectData)
    {
        if (element == ElementType.Ice && CanBeApplied(ElementType.Ice))
            ApplyChillEffect(effectData.chillDuration, effectData.chillslowMultiplier);

        if (element == ElementType.Fire && CanBeApplied(ElementType.Fire))
            ApplyBurnEffect(effectData.burnDuration, effectData.totalBurnDamage);

        if (element == ElementType.Lightning && CanBeApplied(ElementType.Lightning))
            ApplyShockEffect(effectData.shockDuration, effectData.shockDamage, effectData.shockCharge);
    }

    #region ÉÁ»÷Âß¼­
    public void ApplyShockEffect(float duration, float damage, float charge)
    {
        float lightningResistance = stats.GetElementResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightningResistance);
        currentCharge += finalCharge;

        if (currentCharge > maximumCharge)
        {
            DoLightningStrike(damage);
            StopShockEffect();
            return;
        }

        if (shockCo != null)
            StopCoroutine(shockCo);

        shockCo = StartCoroutine(ShockEffectCo(duration));
    }

    private void StopShockEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        vfx.StopAllVfx();
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVfx, transform.position + new Vector3(0, .5f, 0), Quaternion.identity);
        health.ReduceHealth(damage);
    }

    private IEnumerator ShockEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        vfx.PlayOnStatusVfx(duration, ElementType.Lightning);

        yield return new WaitForSeconds(duration);
        StopShockEffect();
    }
    #endregion

    #region ×ÆÉËÂß¼­
    public void ApplyBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = stats.GetElementResistance(ElementType.Fire);
        float finalDamage = fireDamage * (1 - fireResistance);

        StartCoroutine(BurnEffectCo(duration, finalDamage));
    }

    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        currentEffect = ElementType.Fire;
        vfx.PlayOnStatusVfx(duration, ElementType.Fire);

        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);

        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            health.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementType.None;
    }
    #endregion

    #region ±ù¶³Âß¼­
    public void ApplyChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = stats.GetElementResistance(ElementType.Ice);
        float finalDuration = duration * (1 - iceResistance);

        StartCoroutine(ChillEffectCo(finalDuration, slowMultiplier));
    }

    private IEnumerator ChillEffectCo(float duration, float slowMultiplier)
    {
        character.SlowDownEntity(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        vfx.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);
        currentEffect = ElementType.None;
    }
    #endregion
    public bool CanBeApplied(ElementType element)
    {
        if (element == ElementType.Lightning && currentEffect == ElementType.Lightning)
            return true;

        return currentEffect == ElementType.None;
    }
}
