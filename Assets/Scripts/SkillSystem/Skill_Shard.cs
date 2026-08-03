using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;
    private Character_Health playerHealth;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2f;

    [Header("Shard移动")]
    [SerializeField] private float shardSpeed = 7;

    [Header("MutiCast Shard 升级")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isReCharging;

    [Header("Teleport Shard 升级")]
    [SerializeField] private float shardExistDuration = 10f;

    [Header("Health Rewind Shard 升级")]
    [SerializeField] private float savedHealthPercent;

    protected override void Awake()
    {
        base.Awake();
        currentCharges = maxCharges;
        playerHealth = GetComponentInParent<Character_Health>();
    }

    public void CreateShard()
    {
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);

        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);

        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            currentShard.OnExplode += ForceCooldown;
    }

    public void CreateRawShard(Transform target = null, bool shardsCanMove = false)
    {
        bool canMove = shardsCanMove != false ? shardsCanMove :
            Unlocked(SkillUpgradeType.Shard_MoveToEnemy) || Unlocked(SkillUpgradeType.Shard_MultiCast);

        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shard.GetComponent<SkillObject_Shard>().SetupShard(this, detonateTime, canMove, shardSpeed);
    }

    public void CreateDomainShard(Transform target)
    {

    }

    public override void TryUseSkill()
    {
        base.TryUseSkill();
        if (CanUseSkill() == false)
            return;

        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();

        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();

        if (Unlocked(SkillUpgradeType.Shard_MultiCast))
            HandleShardMultiCast();

        if (Unlocked(SkillUpgradeType.Shard_Teleport))
            HandleShardTeleport();

        if (Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            HandleShardHealthRewind();
    }
    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);

        SetSkillOnCooldown();
    }

    //MutiCast Shard 升级
    private void HandleShardMultiCast()
    {
        if (currentCharges <= 0)
            return;

        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharges--;

        if (isReCharging == false)
        {
            StartCoroutine(ShardReChargeCo());
        }
    }
    private IEnumerator ShardReChargeCo()
    {
        isReCharging = true;

        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currentCharges++;
        }

        isReCharging = false;
    }

    //Teleport Shard 升级
    private void HandleShardTeleport()
    {
        if (currentShard == null)
        {
            CreateShard();
        }
        else
        {
            SwapShardAndPlayer();
            SetSkillOnCooldown();
        }
    }

    private void SwapShardAndPlayer()
    {
        Vector3 shardPos = currentShard.transform.position;
        Vector3 playerPos = player.transform.position;

        currentShard.transform.position = playerPos;
        currentShard.Explode();

        player.TeleportPlayer(shardPos);
    }

    //Health Rewind Shard 升级
    private void HandleShardHealthRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercent = playerHealth.GetHealthPercent();
        }
        else
        {
            SwapShardAndPlayer();

            if (playerHealth.GetHealthPercent() < savedHealthPercent)
                playerHealth.SetHealthToPercent(savedHealthPercent);

            SetSkillOnCooldown();
        }
    }


    public float GetDetonateTime()
    {
        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            return shardExistDuration;

        return detonateTime;
    }

    private void ForceCooldown()
    {
        if (OnCooldown() == false)
        {
            SetSkillOnCooldown();
            currentShard.OnExplode -= ForceCooldown;
        }
    }
}
