using System;
using UnityEditor;

[Serializable]
public class ElementalEffectData
{
    public float chillDuration;
    public float chillslowMultiplier;

    public float burnDuration;
    public float totalBurnDamage;

    public float shockDuration;
    public float shockDamage;
    public float shockCharge;
    public ElementalEffectData(Stats_System entityStats, DamageScaleData damageScale)
    {
        chillDuration = damageScale.chillDuration;
        chillslowMultiplier = damageScale.chillslowMulitplier;
        burnDuration = damageScale.burnDuration;
        totalBurnDamage = entityStats.offense.fireDamage.GetValue() * damageScale.burnDamageScale;
        shockDuration = damageScale.shockDuration;
        shockDamage = entityStats.offense.lightningDamage.GetValue() * damageScale.shockDamageScale;
        shockCharge = damageScale.shockCharge;

    }

}

