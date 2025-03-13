

using System.Collections;
using UnityEngine;

public class StateMoveBackward : StateBase
{
    private Vector2 m_Target;
    private float m_Duration = 0.5f;
    private float m_Current = 0;

    public StateMoveBackward(Monster monster) : base(monster)
    {
    }

    public override bool CanExecute => true;

    public override void OnEnterState()
    {
        Debug.Log("back enter");
        m_Target = rigid.position + Vector2.right * 1.8f;

        monster.CanMove = false;
        monster.IsMovingBackward = true;
        //monster.StartCoroutine(MoveBack());
    }

    private IEnumerator MoveBack()
    {
        rigid.position += Vector2.right;
        yield return new WaitForSeconds(1f);
    }

    public override void OnUpdateState()
    {
        Debug.Log("back update");
        Vector2 vel = Vector2.one * 1.5f;

        if (m_Current < m_Duration)
        {
            //rigid.position = Vector2.MoveTowards(rigid.position, rigid.position + Vector2.right, 10f * Time.deltaTime);
            rigid.position = Vector2.Lerp(rigid.position, rigid.position + Vector2.right * 4f, m_Current/m_Duration * Time.deltaTime);
            m_Current += Time.deltaTime;
        }
        else
        {
            machine.ChangeState(StateType.None);

        }
        //rigid.AddForce(Vector2.right * 0.1f, ForceMode2D.Impulse);
        //rigid.position = Vector2.SmoothDamp(rigid.position, m_Target, ref vel, 1f);
        //rigid.velocity = new Vector2(5f * Time.deltaTime, rigid.velocity.y);

    }

    public override void OnExitState()
    {
        Debug.Log("back exit");
        m_Current = 0;
        monster.CanMove = true;
        monster.IsMovingBackward= false;
    }

    
}
