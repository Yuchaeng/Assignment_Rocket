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

    private Rigidbody2D m_Rigid;
    private Vector2 m_MovePosition;
    private float m_MoveSpeedX = -2f;

    private float m_RayDistance = 0.2f;
    private LayerMask m_MyLayer;
    private Vector2 m_RayPosOffset = new(-0.7f, 0.5f);

    private bool m_IsJump = false;
    public bool CanMove { get; set; } = true;
    public bool IsGrounded { get; private set; }

    public bool IsMovingBackward = false;
    private int PlayerLayer;
    public bool CanAttack { get; set; } = true;
    private float m_AttackSpeed = 1f;
    private WaitForSeconds m_WaitforAttack;

    public StateMachine StateMachine;


    private void Start()
    {
        m_Rigid = GetComponent<Rigidbody2D>();
        m_MyLayer = 1 << gameObject.layer;
        PlayerLayer = 1 << LayerMask.NameToLayer("Player");
        m_WaitforAttack = new WaitForSeconds(1 / m_AttackSpeed);

        StateMachine = new StateMachine();
        StateMachine.Init(new Dictionary<StateType, StateBase>
        {
            {StateType.None, new StateNone(this)},
            {StateType.Jump, new StateJump(this)},
            {StateType.MoveBackward, new StateMoveBackward(this)},
            {StateType.Attack, new StateAttack(this)},
        },
        StateType.None);
    }

    private void Update()
    {
        StateMachine.UpdateState();

        if (CanMove)
        {
            m_Rigid.velocity = new Vector2(m_MoveSpeedX, m_Rigid.velocity.y);
        }

        

    }

    private void FixedUpdate()
    {
        if (CanAttack)
        {
            TryAttack();
        }

        if (!IsMovingBackward)
        {
            CheckFrontMonster();
        }

        if (CanMove && !CheckGround())
        {
            
        }
        IsGrounded = CheckGround();
        //Debug.Log(IsGrounded + " " + gameObject.name);

    }

    private void CheckFrontMonster()
    {
        RaycastHit2D hitFront = Physics2D.Raycast(m_Rigid.position + new Vector2(-0.7f, 0.3f), Vector2.left, 0.2f, m_MyLayer);

        if (hitFront)
        {
            //Debug.Log($"앞에 몬스터 {gameObject.name}");
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
            //m_Rigid.gravityScale = 1.5f;
        }
        return hitBottom;
    }

    private void TryAttack()
    {
        RaycastHit2D hitAttack = Physics2D.Raycast((Vector2)transform.position + new Vector2(-0.5f, 0.7f), Vector2.left, 0.3f, PlayerLayer);
        if (hitAttack)
        {
            CanAttack = false;

            //Debug.Log($"attack success {gameObject.name}");
            StateMachine.ChangeState(StateType.Attack);
            StartCoroutine(WaitAttackCoolTime());
        }

    }

    private IEnumerator WaitAttackCoolTime()
    {
        yield return m_WaitforAttack;
        CanAttack = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 lineOrigin = (Vector2)transform.position + new Vector2(-0.7f, 0.3f);
        Gizmos.DrawLine(lineOrigin, lineOrigin + Vector2.left * 0.2f);

        Gizmos.color = Color.white;
        Vector2 hitLine = (Vector2)transform.position + new Vector2(-0.5f, 0.7f);
        Gizmos.DrawLine(hitLine, hitLine + Vector2.left * 0.3f);
    }
#endif

}
