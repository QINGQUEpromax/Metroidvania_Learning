using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    public static event Action OnPlayerDeath;

    public InputActions inputActions { get; private set; }

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
    public Vector2 moveInput { get; private set; }

    [Header("ÒÆ¶¯ÌøÔ¾")]
    public float moveSpeed;
    public float jumpForce;
    [Range(0, 1)]
    public float inAirSpeed;//¿ÕÖÐËÙ¶È³ËÊý
    [Range(0, 1)]
    public float OnWallSpeed;//¹ÒÇ½ËÙ¶È³ËÊý

    private Coroutine queueAttackCo;

    [Header("³å´Ì")]
    public float dashDuration;
    public float dashSpeed;

    [Header("¹¥»÷")]
    public Vector2[] attackForce;
    public Vector2 jumpAttackForce;
    public float attackDuration;
    public float resetInterval;//ÖØÖÃ¹¥»÷

    protected override void Awake()
    {
        base.Awake();

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
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.initialize(idleState);
    }

    //Íæ¼ÒËÀÍö
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
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
