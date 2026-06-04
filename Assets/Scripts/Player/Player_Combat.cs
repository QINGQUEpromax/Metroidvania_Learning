using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Player_Combat : Combat_System
{
    [Header("反击逻辑")]
    public float counterDuration;

    public bool counterOver { get; private set; }
    private Combat_System combat  => GetComponent<Combat_System>();
    private Player player => GetComponent<Player>();
    private Enemy enemy;

    private void Awake()
    {
        enemy = GameObject.Find("Enemy_Skeleton").GetComponent<Enemy>();
    }

    protected override void Update()
    {
        base.Update();
        counterOver = player.anim.GetBool("counterPerformed");
        
    }
    public bool CounterPerformed()
    {
        bool hasCounteredSomebody = false;

        foreach(var target in GetTargetCollider())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            IDamage damagable = target.GetComponent<IDamage>();

            if (counterable == null) 
            {
                continue;
            }

            if(counterable.canBeCountered)
            {
                damagable?.TakeDamage(combat.damage,enemy.stunnedDuration,transform);
                target.gameObject.GetComponent<Character>().CreateHitVfx();
                counterable?.HandleCounter();
                hasCounteredSomebody = true;
            }
        }
        return hasCounteredSomebody;

    }
        //反击结束事件
        private void CounterOver()
        {
            player.anim.SetBool("counterPerformed", false);
        }
}
