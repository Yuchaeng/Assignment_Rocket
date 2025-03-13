using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StateMove : StateBase
{
    private float m_MoveSpeedX = -2f;

    public StateMove(Monster monster) : base(monster) { }

    public override bool CanExecute => true;

    public override void OnEnterState()
    {
        monster.CanMove = true;
    }

    public override void OnUpdateState()
    {
        Debug.Log("move");
        rigid.velocity = new Vector2(m_MoveSpeedX, rigid.velocity.y);

    }

    public override void OnExitState()
    {

    }

   
}
