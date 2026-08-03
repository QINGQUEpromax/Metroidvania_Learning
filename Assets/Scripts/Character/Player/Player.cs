using System;
using System.Collections;
using UnityEngine;

public class Player : Character
{
    public static event Action OnPlayerDeath;

    private UI ui;
    public InputActions inputActions { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    public Player_VFX vfx { get; private set; }
    public Character_Health health { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }


    #region 角色状态变量声明
    public PlayerIdle idleState { get; private set; }
    public PlayerMove moveState { get; private set; }
    public PlayerJump jumpState { get; private set; }
    public PlayerFall fallState { get; private set; }
    public PlayerWallSlide wallSlideState { get; private set; }
    public PlayerWallJump wallJumpState { get; private set; }
    public PlayerDash dashState { get; private set; }
    public PlayerAttack attackState { get; private set; }
    public PlayerJumpAttack jumpAttackState { get; private set; }
    public PlayerDie dieState { get; private set; }
    public PlayerCounter counterState { get; private set; }
    public PlayerSwordThrow swordThrowState { get; private set; }
    public Player_DomainExpansion domainExpansionState { get; private set; }
    public Vector2 moveInput { get; private set; }
    public Vector2 mousePosition { get; private set; }
    #endregion

    [Header("Move and Jump")]
    public float moveSpeed;
    public float jumpForce;
    [Range(0, 1)]
    public float inAirSpeed;//空中速度乘数
    [Range(0, 1)]
    public float OnWallSpeed;//挂墙速度乘数
    private Coroutine queueAttackCo;

    [Header("Ultimate ability details")]
    public float riseSpeed = 25;
    public float riseMaxDistance = 3;

    [Header("Dash details")]
    public float dashDuration;
    public float dashSpeed;

    [Header("Attack details")]
    public Vector2[] attackForce;
    public Vector2 jumpAttackForce;
    public float attackDuration;
    public float resetInterval;//重置攻击

    protected override void Awake()
    {
        base.Awake();

        ui = FindAnyObjectByType<UI>();
        skillManager = GetComponent<Player_SkillManager>();
        vfx = GetComponent<Player_VFX>();
        health = GetComponent<Character_Health>();
        statusHandler = GetComponent<Entity_StatusHandler>();

        inputActions = new InputActions();
       
        idleState = new PlayerIdle(this, stateMachine, "idle");
        moveState = new PlayerMove(this, stateMachine, "move");
        jumpState = new PlayerJump(this, stateMachine, "jumpFall");
        fallState = new PlayerFall(this, stateMachine, "jumpFall");
        wallSlideState = new PlayerWallSlide(this, stateMachine, "wallSlide");
        wallJumpState = new PlayerWallJump(this, stateMachine, "jumpFall");
        dashState = new PlayerDash(this, stateMachine, "dash");
        attackState = new PlayerAttack(this, stateMachine, "attack");
        jumpAttackState = new PlayerJumpAttack(this, stateMachine, "jumpAttack");
        dieState = new PlayerDie(this, stateMachine, "die");
        counterState = new PlayerCounter(this, stateMachine, "counter");
        swordThrowState = new PlayerSwordThrow(this, stateMachine, "swordThrow");
        domainExpansionState = new Player_DomainExpansion(this, stateMachine, "jumpFall");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.initialize(idleState);
    }

    //传送玩家
    public void TeleportPlayer(Vector3 destination) => transform.position = destination;

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;
        Vector2 originalwallJump = wallJumpForce;
        Vector2 originalJumpAttack = jumpAttackForce;
        Vector2[] originalAttackForce = new Vector2[attackForce.Length];
        Array.Copy(attackForce, originalAttackForce, attackForce.Length);

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed = moveSpeed * speedMultiplier;
        jumpForce = jumpForce * speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;
        wallJumpForce = wallJumpForce * speedMultiplier;
        jumpAttackForce = jumpAttackForce * speedMultiplier;
        for (int i = 0; i < attackForce.Length; i++)
        {
            attackForce[i] = attackForce[i] * speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        anim.speed = originalAnimSpeed;
        wallJumpForce = originalwallJump;
        jumpAttackForce = originalJumpAttack;

        for (int i = 0; i < attackForce.Length; i++)
        {
            attackForce[i] = originalAttackForce[i];

        }
    }
    //玩家死亡
    public override void CharacterDeath()
    {
        base.CharacterDeath();

        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(dieState);
    }

    public void EnterAttackStateWithDelay()
    {
        if (queueAttackCo != null)
            StopCoroutine(queueAttackCo);

        queueAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(attackState);
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Mouse.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTreeUI();
        inputActions.Player.Spell.performed += ctx => Player_SkillManager.instance.shard.TryUseSkill();
        inputActions.Player.Spell.performed += ctx => Player_SkillManager.instance.timeEcho.TryUseSkill();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
