using UnityEngine;

public class StateJump : StateBase
{
    private float m_JumpForce = 4.2f;
    private float m_JumpTime = 0.5f;
    private float m_currentTime = 0f;

    public StateJump(Monster monster) : base(monster)
    {
    }

    public override bool CanExecute => true;

    public override void OnEnterState(object obj = null)
    {
        monster.CanMove = true;
        monster.IsJumping = true;

        rigid.velocity = Vector2.zero;

        // 계속 이동 중이므로 위로만 점프 후 자연스럽게 앞으로 이동
        rigid.AddForce(Vector2.up * m_JumpForce, ForceMode2D.Impulse);
    }

    public override void OnUpdateState()
    {
        if (rigid.velocity.y < 0)
        {
            rigid.gravityScale = 1.5f;
        }

        m_currentTime += Time.deltaTime;
        if (m_currentTime >= m_JumpTime)
        {
            machine.ChangeState(StateType.Default);
            return;
        }
    }

    public override void OnExitState()
    {
        monster.IsJumping = false;
        monster.CanMove = false;
        m_currentTime = 0f;
        rigid.gravityScale = 1f;
        monster.SetJumpCoolTime();
    }

}
