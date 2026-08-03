using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill Data - ")]

public class Skill_DataSO : ScriptableObject
{

    [Header("技能描述")]
    public string displayName;
    [TextArea] 
    public string description;
    public Sprite icon;

    [Header("技能解锁与升级")]
    public int cost;
    public bool unlockedByDefault;
    public SkillType skillType;
    public UpgradeData upgradeData;

}

[Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
    public DamageScaleData damageScaleData;
}


