using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Character character;
    private Character_VFX vfx;
    private Stats_System stats;
    private Character_Health health;
    private ElementType currentEffect = ElementType.None;

    private void Awake()
    {
        health = GetComponent<Character_Health>();
        character = GetComponent<Character>();
        stats = GetComponent<Stats_System>();
        vfx = GetComponent<Character_VFX>();
    }

    [Header("ÉÁ»÷Âß¼­")]
    [SerializeField] private GameObject lightningStrikeVfx;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximumCharge = 1;
    private Coroutine electrifyCo;

    #region ÉÁ»÷Âß¼­
    public void ApplyElectrifyEffect(float duration,float damage,float charge)
    {
        float lightningResistance = stats.GetElementResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightningResistance);
        currentCharge += finalCharge;

        if (currentCharge > maximumCharge)
        {
            DoLightningStrike(damage);
            StopElectrifyEffect();
            return;
        }

        if (electrifyCo != null)
            StopCoroutine(electrifyCo);

        electrifyCo = StartCoroutine(ElectrifyEffectCo(duration));
    }

    private void StopElectrifyEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        vfx.StopAllVfx();
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVfx, transform.position + new Vector3(0,.5f,0), Quaternion.identity);
        health.ReduceHealth(damage);
    }

    private IEnumerator ElectrifyEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        vfx.PlayOnStatusVfx(duration, ElementType.Lightning);

        yield return new WaitForSeconds(duration);
        StopElectrifyEffect();
    }
    #endregion

    #region ×ÆÉËÂß¼­
    public void ApplyBurnEffect(float duration,float fireDamage)
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

        for(int i = 0; i < tickCount; i++)
        {
            health.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementType.None;
    }
    #endregion

    #region ±ù¶³Âß¼­
    public void ApplyChillEffect(float duration,float slowMultiplier)
    {
        float iceResistance = stats.GetElementResistance(ElementType.Ice);
        float finalDuration = duration * (1 - iceResistance);

        StartCoroutine(ChillEffectCo(finalDuration,slowMultiplier));
    }

    private IEnumerator ChillEffectCo(float duration,float slowMultiplier)
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
        if(element == ElementType.Lightning && currentEffect == ElementType.Lightning)
            return true;

        return currentEffect == ElementType.None;
    }
}
