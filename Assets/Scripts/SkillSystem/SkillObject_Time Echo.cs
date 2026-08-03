using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private float wispMoveSpeed = 15;
    [SerializeField] private GameObject onDeathVfx;
    [SerializeField] private LayerMask groundLayer;
    private bool shouldMoveToPlayer;

    private Transform playerTransform;
    private Skill_TimeEcho echoManager;
    private TrailRenderer wispTrail;
    private Character_Health playerHealth;
    private SkillObject_Health echoHealth;
    private Entity_StatusHandler statusHandler;

    public int maxAttacks { get; private set; }

    public void SetupEcho(Skill_TimeEcho echoManager)
    {
        this.echoManager = echoManager;
        playerStats = echoManager.player.stats;
        damageScaleData = echoManager.damageScaleData;
        maxAttacks = echoManager.GetMaxAttacks();
        playerTransform = echoManager.transform.root;
        playerHealth = echoManager.player.health;
        statusHandler = echoManager.player.statusHandler;

        Invoke(nameof(HandleDeath), echoManager.GetEcnoDuration());
        FlipToTarget();

        echoHealth = GetComponent<SkillObject_Health>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        anim.SetBool("canAttack", maxAttacks > 0);
    }

    private void Update()
    {
        if (shouldMoveToPlayer)
            HandleWispMovement();
        else
        {
            anim.SetFloat("yVelocity", rb.velocity.y);
            StopHorizontalMovement();
        }
    }

    private void HandlePlayerTouch()
    {
        float healAmount = echoHealth.lastDamageTaken * echoManager.GetPercentOfDamageHealed();
        playerHealth.IncreaseHealth(healAmount);

        float amountInSeconds= echoManager.GetCooldownReduceInSeconds();
        Player_SkillManager.instance.ReduceAllskillcooldownBy(amountInSeconds);

        if (echoManager.CanRemoveNegativeEffects())
            statusHandler.RemoveAllNegativeEffects();
    }

    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, wispMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, playerTransform.position) < .5f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }


    private void FlipToTarget()
    {
        Transform target = FindClosestTarget();

        if (target != null && target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }

    public void PerformAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);
        if (lastTarget == null)
            return;

        bool canDuplicate = Random.value < echoManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;

        if (canDuplicate)
            echoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0));
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVfx, transform.position, Quaternion.identity);

        if (echoManager.ShouldBeWisp())
            TurnIntoWisp();
        else
            Destroy(gameObject);
    }

    private void TurnIntoWisp()
    {
        shouldMoveToPlayer = true;
        sr.enabled = false;
        wispTrail.gameObject.SetActive(true);
        rb.simulated = false;
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);

        if (hit.collider != null)
            rb.velocity = new Vector2(0, rb.velocity.y);

    }
}
