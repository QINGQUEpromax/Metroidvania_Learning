using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InputActions inputActions { get; private set; }

    private StateMachine stateMachine;

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public PlayerIdle idleState { get; private set; }
    public PlayerMove moveState { get; private set; }
    public PlayerJump jumpState { get; private set; }
    public PlayerFall fallState { get; private set; }
    public PlayerWallSlide wallSlideState { get; private set; }
    public PlayerWallJump wallJumpState { get; private set; }
    public PlayerDash dashState { get; private set; }
    public PlayerAttack attackState { get; private set; }
    public PlayerJumpAttack jumpAttackState {  get; private set; }
    public Vector2 moveInput { get; private set; }

    [Header("移动跳跃")]
    public float moveSpeed;
    public float jumpForce;
    [Range(0, 1)]
    public float inAirSpeed;//空中速度乘数
    [Range(0, 1)]
    public float OnWallSpeed;//挂墙速度乘数
    private bool facingright = true;

    private Coroutine queueAttackCo;

    [Header("冲刺")]
    public float dashDuration;
    public float dashSpeed;

    [Header("攻击")]
    public Vector2[] attackForce;
    public Vector2 jumpAttackForce;
    public float attackDuration;
    public float resetInterval;//重置攻击

    [Header("接地触墙检测")]
    [SerializeField] private Vector2 offset;//偏移量
    [SerializeField] private float groundDetectRadius;//检测半径
    [SerializeField] private float wallDetectDistance;//检测距离墙体距离
    [SerializeField] private Transform firstDetectRay;//第一个墙体检测射线起始位置
    [SerializeField] private Transform secondDetectRay;//第二个墙体检测射线起始位置
    public LayerMask groundLayer;
    public Vector2 wallJumpForce;
    public int facingDir { get; private set; } = 1;
    public bool isGrounded { get; private set; }
    public bool isOnWall { get; private set; }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        inputActions = new InputActions();
        stateMachine = new StateMachine();

        idleState = new PlayerIdle(this, stateMachine, "idle");
        moveState = new PlayerMove(this, stateMachine, "move");
        jumpState = new PlayerJump(this, stateMachine, "jumpFall");
        fallState = new PlayerFall(this, stateMachine, "jumpFall");
        wallSlideState = new PlayerWallSlide(this, stateMachine, "wallSlide");
        wallJumpState = new PlayerWallJump(this, stateMachine, "jumpFall");
        dashState = new PlayerDash(this, stateMachine, "dash");
        attackState = new PlayerAttack(this, stateMachine, "attack");
        jumpAttackState = new PlayerJumpAttack(this, stateMachine, "jumpAttack");
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

    private void Start()
    {
        stateMachine.initialize(idleState);

    }

    private void Update()
    {
        stateMachine.UpdateActiveState();
        DetectIsGrounded();
    }

    public void EnterAttackStateWithDelay()
    {
        if(queueAttackCo != null)
            StopCoroutine(queueAttackCo);

        queueAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(attackState);
    }

    #region 基本移动逻辑
    //设置人物移动速度
    public void SetVelocity(float xSpeed,float ySpeed)
    {
        rb.velocity = new Vector2(xSpeed, ySpeed);
        HandleFlip(xSpeed);
    }

    //人物翻转
    private void HandleFlip(float xSpeed)
    {
        if (facingright && xSpeed < 0)
            Flip();
        if (facingright == false && xSpeed > 0)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingright = !facingright;
        facingDir = facingDir * -1;
    }
    #endregion

    //攻击事件
    private void AttackOver()
    {
        stateMachine.currentState.attackOver = true;
    } 
    
    //接地检测
    private void DetectIsGrounded()
    {
        Vector2 pos = transform.position;
        isGrounded = Physics2D.OverlapCircle(pos + offset,groundDetectRadius, groundLayer);
        isOnWall = Physics2D.Raycast(firstDetectRay.position,Vector2.right * facingDir,wallDetectDistance,groundLayer)
                && Physics2D.Raycast(secondDetectRay.position, Vector2.right * facingDir, wallDetectDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(firstDetectRay.position, firstDetectRay.position + new Vector3(facingDir * wallDetectDistance, 0,0));
        Gizmos.DrawLine(secondDetectRay.position, secondDetectRay.position + new Vector3(facingDir * wallDetectDistance, 0,0));
        Vector2 pos = transform.position;
        Gizmos.DrawWireSphere(pos + offset, groundDetectRadius);
    }
}
