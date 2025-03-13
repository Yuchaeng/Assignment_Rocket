using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class StateAttack : StateBase
{
    private float cur = 0;
    private float dur = 0.5f;

    public StateAttack(Monster monster) : base(monster)
    {
    }

    public override bool CanExecute => true;

    public override void OnEnterState()
    {
        monster.CanMove = true;
        Debug.Log("Attack!!!!!!!");
        // 애니메이션 바꾸기

    }
    public override void OnUpdateState()
    {
       // after animation ended, 상태 변화
       if (cur >= dur)
        {
            
            machine.ChangeState(StateType.None);
        }
        else
        {
            cur += Time.deltaTime;
        }
    }

    public override void OnExitState()
    {
    }


}
