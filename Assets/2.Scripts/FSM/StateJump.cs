using UnityEngine;

public class StateJump : StateBase
{
    private float m_JumpForce = 3.8f;

    public StateJump(Monster monster) : base(monster)
    {
    }

    public override bool CanExecute => true;

    public override void OnEnterState(object obj = null)
    {
        monster.CanMove = true;
        monster.IsJumping = true;
        rigid.velocity = Vector2.zero;

        // 계속 이동 중이므로 위로만 점프해도 대각선 방향으로 이동 가능
        rigid.AddForce(Vector2.up * m_JumpForce, ForceMode2D.Impulse);
    }

    public override void OnUpdateState()
    {
        if (monster.IsGrounded)
        {
            machine.ChangeState(StateType.Default);
            return;
        }
    }

    public override void OnExitState()
    {
        monster.IsJumping = false;
    }

}
