using System;
using System.Collections;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public event Action OnFlipped;
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Stats_System stats { get; private set; }
    public StateMachine stateMachine { get; private set; }

    public Combat_System combat { get; private set; }

    private bool facingright = true;

    [Header("打击特效")]
    public GameObject hitVfx;
    public GameObject onCritHitVfx;
    public Transform vfxCreatedPos;//随机生成中心
    public Vector2 leftBottom;//左下限制
    public Vector2 rightUp;//右上限制

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

    //受伤击退协程
    private bool isKnockback;
    private Coroutine knockbackCo;
    private Coroutine slowDownCo;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats_System>();
        stateMachine = new StateMachine();

    }

    protected virtual void Start()
    {

    }

    private void Update()
    {
        stateMachine.UpdateActiveState();
        DetectIsGrounded();
    }


    //受伤击退协程
    public void Knockback(Vector2 knockback, float knockbackDuration)
    {
        if (knockbackCo != null)
            StopCoroutine(knockbackCo);

        knockbackCo = StartCoroutine(KnockbackCo(knockback, knockbackDuration));
    }

    private IEnumerator KnockbackCo(Vector2 knockback, float knockbackDuration)
    {
        isKnockback = true;
        rb.velocity = knockback;

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = Vector2.zero;
        isKnockback = false;
    }

    #region 基本移动逻辑
    //设置人物移动速度
    public void SetVelocity(float xSpeed, float ySpeed)
    {
        if (isKnockback)
            return;

        rb.velocity = new Vector2(xSpeed, ySpeed);
        HandleFlip(xSpeed);
    }

    //人物翻转
    public void HandleFlip(float xSpeed)
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

        OnFlipped?.Invoke();
    }
    #endregion


    //人物死亡
    public virtual void CharacterDeath()
    {

    }

    //减缓对方速度
    public virtual void SlowDownEntity(float duration, float slowMultiplier, bool canOverrideSlowEffect = false)
    {
        if (slowDownCo != null)
        {
            if (canOverrideSlowEffect)
                StopCoroutine(slowDownCo);
            else
                return;
        }

        slowDownCo = StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));
    }

    protected virtual IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        yield return null;
    }

    public virtual void StopSlowDown()
    {
        slowDownCo = null;
    }

    //接地检测
    protected virtual void DetectIsGrounded()
    {
        Vector2 pos = transform.position;

        isGrounded = Physics2D.OverlapCircle(pos + offset, groundDetectRadius, groundLayer);
        if (secondDetectRay != null)
        {
            isOnWall = Physics2D.Raycast(firstDetectRay.position, Vector2.right * facingDir, wallDetectDistance, groundLayer)
                    && Physics2D.Raycast(secondDetectRay.position, Vector2.right * facingDir, wallDetectDistance, groundLayer);
        }
        else
        {
            isOnWall = Physics2D.Raycast(firstDetectRay.position, Vector2.right * facingDir, wallDetectDistance, groundLayer);
        }

    }

    //绘图
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(firstDetectRay.position, firstDetectRay.position + new Vector3(facingDir * wallDetectDistance, 0, 0));
        if (secondDetectRay != null)
        {
            Gizmos.DrawLine(secondDetectRay.position, secondDetectRay.position + new Vector3(facingDir * wallDetectDistance, 0, 0));
        }

        Vector2 pos = transform.position;
        Gizmos.DrawWireSphere(pos + offset, groundDetectRadius);
    }
}
