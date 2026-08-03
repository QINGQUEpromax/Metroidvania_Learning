using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skillManager { get; private set; }
    public Player player {  get; private set; }
    public DamageScaleData damageScaleData {  get; private set; }

    [Header("Í¨ÓÃÂß¼­")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        skillManager = GetComponentInParent<Player_SkillManager>();
        lastTimeUsed = lastTimeUsed - cooldown;
        player = GetComponentInParent<Player>();
        damageScaleData = new DamageScaleData();
    }

    public virtual void TryUseSkill()
    {

    } 

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        damageScaleData = upgrade.damageScaleData;
        ResetCooldown();
    }

    public virtual bool CanUseSkill()
    {
        if(upgradeType == SkillUpgradeType.None)
            return false;

        if (OnCooldown())
            return false;

        return true;
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;

    public bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ReduceCooldownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;
    public void ResetCooldown() => lastTimeUsed = Time.time - cooldown;
}
