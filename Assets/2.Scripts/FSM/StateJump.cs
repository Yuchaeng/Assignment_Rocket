using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class StateJump : StateBase
{
    private float m_JumpForce = 3.8f;
    private Vector2 m_Direction;

    public StateJump(Monster monster) : base(monster)
    {
    }

    public override bool CanExecute => true;

    public override void OnEnterState()
    {
        monster.CanMove = true;
        rigid.velocity = Vector2.zero;

        // 계속 이동 중이므로 위로만 점프해서 대각선 방향으로 점프하도록
        m_Direction = new Vector2(0, collider.bounds.size.y + 0.1f);
        rigid.AddForce(Vector2.up * m_JumpForce, ForceMode2D.Impulse);

        Debug.Log($"jump {monster.name}");

    }

    public override void OnUpdateState()
    {
        //RaycastHit2D hitBottom = Physics2D.Raycast(monster.transform.position, Vector2.down, 0.1f);
        
        if (monster.IsGrounded)
        {
            //Debug.Log($"땅에 있음 {monster.name}");
            machine.ChangeState(StateType.None);

            return;
        }
        else
        {
            Debug.Log($"땅에 닿지 않음 {monster.name}");
            if (Mathf.Abs(rigid.velocity.y) < 0.1f)
            rigid.gravityScale = 1.5f;
        }
    }

    public override void OnExitState()
    {
        rigid.gravityScale = 1f;
    }

}
