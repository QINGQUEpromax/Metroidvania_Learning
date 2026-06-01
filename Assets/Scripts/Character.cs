using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    protected StateMachine stateMachine;

    private bool facingright = true;

    [Header("接地触墙检测")]
    [SerializeField] private Vector2 offset;//偏移量
    [SerializeField] private float groundDetectRadius;//检测半径
    [SerializeField] private float aheadCheckDistance;//敌人检测前方地面距离
    [SerializeField] private float wallDetectDistance;//检测距离墙体距离
    [SerializeField] private Transform roadAheadCheck;//检测敌人前方是否有路
    [SerializeField] private Transform firstDetectRay;//第一个墙体检测射线起始位置
    [SerializeField] private Transform secondDetectRay;//第二个墙体检测射线起始位置
    public LayerMask groundLayer;
    public Vector2 wallJumpForce;
    public int facingDir { get; private set; } = 1;
    public bool isGrounded { get; private set; }
    public bool isOnWall { get; private set; }
    public bool haveRoadAhead { get; private set; }//敌人检测前方是否有路

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

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

   

    #region 基本移动逻辑
    //设置人物移动速度
    public void SetVelocity(float xSpeed, float ySpeed)
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
        haveRoadAhead = Physics2D.Raycast(roadAheadCheck.position, Vector2.down , aheadCheckDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(firstDetectRay.position, firstDetectRay.position + new Vector3(facingDir * wallDetectDistance, 0, 0));
        if (secondDetectRay != null)
        {
            Gizmos.DrawLine(secondDetectRay.position, secondDetectRay.position + new Vector3(facingDir * wallDetectDistance, 0, 0));
        }
        Gizmos.DrawLine(roadAheadCheck.position, roadAheadCheck.position +  Vector3.down * facingDir * aheadCheckDistance);//敌人
        Vector2 pos = transform.position;
        Gizmos.DrawWireSphere(pos + offset, groundDetectRadius);
    }
}
