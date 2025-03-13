using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // 몬스터 3줄 나눠서 스폰
    // 같은 줄끼리만 상호작용
    // 가다가 충돌하면(앞에 몬스터가 있으면) 걍 점프
    // 올라타면서 앞으로 계속 이동. 이동하다가 떨어지고 밑에 있는 애는 뒤로 밀려남
    // 뒤로 가기할 땐 탐지 안함

    // 1. 이동, 2. 점프, 3. 내려오기(중력으로 그냥??) 4. 밀려나기 5. 공격 6. 사망

    private Rigidbody2D m_Rigid;
    private Vector2 m_MovePosition;
    private float m_MoveSpeedX = -2f;

    private Collider2D m_Collider;

    private float m_RayDistance = 0.2f;
    private LayerMask m_MyLayer;
    private Vector2 m_RayPosOffset = new(-0.7f, 0.5f);

    private bool m_IsJump = false;
    public bool CanMove { get; set; } = true;
    public bool IsGrounded { get; private set; }
    private int m_GroundLayer;

    public bool IsMovingBackward = false;
    public StateMachine StateMachine;


    private void Start()
    {
        m_Rigid = GetComponent<Rigidbody2D>();
        m_MyLayer = 1 << gameObject.layer;
        m_GroundLayer = LayerMask.NameToLayer("Ground");

        StateMachine = new StateMachine();
        StateMachine.Init(new Dictionary<StateType, StateBase>
        {
            {StateType.None, new StateNone(this)},
            {StateType.Jump, new StateJump(this)},
            {StateType.MoveBackward, new StateMoveBackward(this)},
        },
        StateType.None);
    }

    private void Update()
    {
        StateMachine.UpdateState();
        Debug.Log($"{StateMachine.CurrentType}, {gameObject.name}");

        if (CanMove)
        {
            Debug.Log($"이동 중 {gameObject.name}");
            m_Rigid.velocity = new Vector2(m_MoveSpeedX, m_Rigid.velocity.y);
        }

        //RaycastHit2D hitBottom = Physics2D.Raycast(m_Rigid.position, Vector2.down, m_RayDistance);
        //if (!hitBottom && Mathf.Abs(m_Rigid.velocity.y) < 0.1f)
        //{
        //    CanMove = false;
        //    m_Rigid.velocity = Vector2.down;
        //    Invoke("SetMove", 0.5f);
        //}

    }

    private void FixedUpdate()
    {
        //RaycastHit2D hitUpper = Physics2D.BoxCast(m_Rigid.position + new Vector2(-0.3f, 1f), new Vector2(0.5f, 0.05f), 0f, Vector2.up, 0.05f, m_MyLayer);

        //if (hitUpper && !m_IsMovingBackward)
        //{
        //    m_IsMovingBackward = true;
        //    StateMachine.ChangeState(StateType.MoveBackward);
        //    Debug.Log("뒤로가기");
        //}

        //if (m_IsMovingBackward && !hitUpper)
        //{
        //    StateMachine.ChangeState(StateType.None);
        //    CanMove = true;
        //    m_IsMovingBackward = false;
        //    Debug.Log("원복");
        //}

        if (!IsMovingBackward)
        {
            CheckFrontMonster();

        }

        if (StateMachine.CurrentType == StateType.Jump)
        {
            
        }
        IsGrounded = CheckGround();
        Debug.Log(IsGrounded + " " + gameObject.name);

    }

    private void CheckFrontMonster()
    {
        RaycastHit2D hitFront = Physics2D.Raycast(m_Rigid.position + new Vector2(-0.7f, 0.3f), Vector2.left, 0.2f, m_MyLayer);

        if (hitFront)
        {
            Debug.Log($"앞에 몬스터 {gameObject.name}");
            StateMachine.ChangeState(StateType.Jump);
        }

    }

    private bool CheckGround()
    {
        RaycastHit2D hitBottom = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        Debug.DrawRay(transform.position, Vector2.down * 0.1f, Color.magenta);

        if (hitBottom)
        {
            IsGrounded = true;
            m_Rigid.gravityScale = 1;
        }
        else
        {
            IsGrounded = false;
            m_Rigid.gravityScale = 1.5f;
        }
        return hitBottom;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(-0.3f, 1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(origin, new Vector2(0.5f, 0.05f));

        Gizmos.color = Color.blue;
        Vector2 lineOrigin = (Vector2)transform.position + new Vector2(-0.7f, 0.3f);
        Gizmos.DrawLine(lineOrigin, lineOrigin + Vector2.left * 0.2f);
    }
#endif

}
