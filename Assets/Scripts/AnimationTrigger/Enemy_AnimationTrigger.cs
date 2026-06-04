using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationTrigger : MonoBehaviour
{
    private Enemy enemy;
    private Enemy_VFX vfx;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        vfx = GetComponent<Enemy_VFX>();
    }

    public void EnableCounterWindows()
    {
        vfx.EnableAttackAlert(true);
        enemy.EnableCounterWindow(true);
    }

    public void DisableCounterWindows()
    {
        vfx.EnableAttackAlert(false);
        enemy.EnableCounterWindow(false);
    }
}
