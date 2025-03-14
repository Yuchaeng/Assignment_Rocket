using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster : CharacterBase
{
    private Rigidbody2D m_Rigid;
    private float m_MoveSpeedX = -2f;

    private int m_MyLayer;
    private int m_HeroLayer;

    /* 상태 확인 bool 프로퍼티들 */
    public bool CanMove { get; set; } = true;
    public bool IsJumping { get; set; } = false;
    public bool IsGrounded { get; private set; } = true;
    public bool IsMovingBackward { get; set; } = false;
    public bool CanAttack { get; private set; } = true;

    private float m_AttackSpeed = 1f;
    private WaitForSeconds m_WaitforAttack;

    [HideInInspector] public StateMachine StateMachine;
    private MonsterHpBar m_HpBar;
    private bool m_IsFirstDamage = true;

    [SerializeField] private SpriteRenderer m_HeadSprite;
    [SerializeField] private SpriteRenderer m_BodySprite;


    private void Start()
    {
        m_Rigid = GetComponent<Rigidbody2D>();
        m_MyLayer = 1 << gameObject.layer;
        m_HeroLayer = 1 << LayerMask.NameToLayer("Hero");
        m_WaitforAttack = new WaitForSeconds(1 / m_AttackSpeed);

        InitStateMachine();
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
        if (CanAttack && !IsJumping)
        {
            TryAttack();
        }

        // 뒤로 가는 중에는 점프하지 않음
        if (!IsMovingBackward)
        {
            TryJump();
        }

        if (IsJumping)
        {
            IsGrounded = CheckGround();
        }
    }

    private void InitStateMachine()
    {
        StateMachine = new StateMachine();
        StateMachine.Init(new Dictionary<StateType, StateBase>
        {
            {StateType.Default, new StateDefault(this)},
            {StateType.Jump, new StateJump(this)},
            {StateType.MoveBackward, new StateMoveBackward(this)},
            {StateType.Attack, new StateAttack(this)},
        },
        StateType.Default);
    }

    /// <summary>
    /// 생성 시 랜덤으로 머리와 몸 이미지를 변경합니다.
    /// </summary>
    /// <param name="newHead"></param>
    /// <param name="newBody"></param>
    public void ChangeAppearance(Sprite newHead, Sprite newBody)
    {
        m_HeadSprite.sprite = newHead;
        m_BodySprite.sprite = newBody;
    }

    public void SetHpBar(GameObject hpBar)
    {
        MaxHp = 30;
        CurrentHp = 30;

        m_HpBar = hpBar.GetComponent<MonsterHpBar>();
        m_HpBar.GetSliderComp();
        m_HpBar.InitHpBar(this);
    }

    private void TryJump()
    {
        // 앞의 몬스터 감지하면 점프
        RaycastHit2D hitFront = Physics2D.Raycast(m_Rigid.position + new Vector2(-0.7f, 0.3f), Vector2.left, 0.2f, m_MyLayer);

        if (hitFront)
        {
            StateMachine.ChangeState(StateType.Jump);
        }
    }

    private bool CheckGround()
    {
        RaycastHit2D hitBottom = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        Debug.DrawRay(transform.position, Vector2.down * 0.1f, Color.magenta);

        return hitBottom;
    }

    private void TryAttack()
    {
        RaycastHit2D hitAttack = Physics2D.Raycast((Vector2)transform.position + new Vector2(-0.5f, 0.7f), Vector2.left, 0.3f, m_HeroLayer);

        if (hitAttack)
        {
            CanAttack = false;
            StateMachine.ChangeState(StateType.Attack, hitAttack);
            StartCoroutine(WaitAttackCoolTime());
        }
    }

    private IEnumerator WaitAttackCoolTime()
    {
        yield return m_WaitforAttack;
        CanAttack = true;
    }

    /// <summary>
    /// Layer에 따른 풀타입을 알아냅니다.
    /// </summary>
    /// <returns></returns>
    private EPoolType GetMonsterPoolType()
    {
        switch (gameObject.layer)
        {
            case 7:
                return EPoolType.Monster1;
            case 8:
                return EPoolType.Monster2;
            case 9:
                return EPoolType.Monster3;
            default:
                return EPoolType.None;
        }
    }

    public override void OnDamaged(float damage)
    {
        if (m_IsFirstDamage)
        {
            m_IsFirstDamage = false;
            m_HpBar.SetActiveBar();
        }

        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            OnDead();
        }
    }

    public override void OnDead()
    {
        m_HpBar.RemoveHpEvent();
        InitForReuse();

        ObjectPoolManager.Instance.ReleaseToPool(EPoolType.HpBar, m_HpBar.gameObject);
        ObjectPoolManager.Instance.ReleaseToPool(GetMonsterPoolType(), gameObject);
    }

    private void InitForReuse()
    {
        CanMove = true;
        IsJumping = false;
        IsGrounded = true;
        IsMovingBackward = false;
        CanAttack = true;
        m_IsFirstDamage = true;

        StateMachine.CurrentType = StateType.Default;
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
