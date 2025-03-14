

using System.Collections;
using UnityEngine;

public class StateMoveBackward : StateBase
{
    private float m_Duration = 0.5f;
    private float m_Current = 0;
    private float m_MoveDistance = 4.5f;

    public StateMoveBackward(Monster monster) : base(monster)
    {
    }

    public override bool CanExecute => true;

    public override void OnEnterState(object obj = null)
    {
        monster.CanMove = false;
        monster.IsMovingBackward = true;
    }


    public override void OnUpdateState()
    {
        if (m_Current < m_Duration)
        {
            rigid.position = Vector2.Lerp(rigid.position, rigid.position + Vector2.right * m_MoveDistance, m_Current / m_Duration * Time.deltaTime);
            m_Current += Time.deltaTime;
        }
        else
        {
            machine.ChangeState(StateType.Default);
        }
    }

    public override void OnExitState()
    {
        m_Current = 0;
        monster.CanMove = true;
        monster.IsMovingBackward= false;
    }

}
