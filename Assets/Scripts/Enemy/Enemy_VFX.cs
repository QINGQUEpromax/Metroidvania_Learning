using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_VFX : Character_VFX
{
    [Header("·´»÷ÌØÐ§")]
    [SerializeField] private GameObject attackAlert;

    protected override void Awake()
    {
        base.Awake();
        attackAlert.SetActive(false);
    }
    public void EnableAttackAlert(bool enable) 
    {
        if (attackAlert == null)
            return; 
        attackAlert.SetActive(enable); 
    }
}
