using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Stats_System
{
    private List<string> activeBuff = new List<string>();
    private Inventory_Player inventory;

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<Inventory_Player>();
    }

    public bool CanApplyBuffof(string source)
    {
        return activeBuff.Contains(source) == false;

    }

    public void ApplyBuff(BuffEffectData[] buffsToAply, float duration, string source)
    {

        StartCoroutine(Buffco(buffsToAply, duration, source));
    }

    private IEnumerator Buffco(BuffEffectData[] buffsToApply, float duration, string source)
    {

        activeBuff.Add(source);

        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).AddModifier(buff.value, source);
        }

        yield return new WaitForSeconds(duration);

        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).RemoveModifier(source);
        }
        
        inventory.TriggerUpdateUI();
        activeBuff.Remove(source);
    }
}
