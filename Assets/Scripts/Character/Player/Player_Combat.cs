using UnityEngine;

public class Player_Combat : Combat_System
{
    [Header("反击逻辑")]
    public float counterDuration;

    public bool counterOver { get; private set; }
    private Combat_System combat => GetComponent<Combat_System>();
    private Player player => GetComponent<Player>();
    private Stats_System stats => GetComponent<Stats_System>();
    private Enemy enemy;

    private void Awake()
    {
        //enemy = GameObject.Find("Enemy_Skeleton").GetComponent<Enemy>();
    }

    protected override void Update()
    {
        base.Update();
        counterOver = player.anim.GetBool("counterPerformed");

    }
    public bool CounterPerformed()
    {
        bool hasCounteredSomebody = false;

        foreach (var target in GetTargetCollider())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (counterable == null)
            {
                continue;
            }

            if (counterable.canBeCountered)
            {

                float elementDamage = stats.GetElementDamage(out ElementType element);
                float damage = stats.GetPhysicalDamage(out bool isCrit);
                damagable?.TakeDamage(damage, elementDamage, element, enemy.stunnedDuration, transform);
                target.gameObject.GetComponent<Character_VFX>().CreateHitVfx(isCrit, element);
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

    //SwordThrowd动画事件
    private void ThrowSword()
    {
        Player_SkillManager.instance.swordThrow.ThrowSword();
    }
}
