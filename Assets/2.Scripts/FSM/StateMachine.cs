

using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    // 상태 돌릴 머신
    // Statebase - 각 상태가 상속?
    // 상태별 행동 작성할 함수, 상태 변화, update에서 돌리기, 몬스터 갖다 쓰기, 서로 이어주기
    public StateType CurrentType;
    public StateBase CurrentState;
    public Dictionary<StateType, StateBase> States;

    public StateMachine()
    {
    }

    public void Init(Dictionary<StateType, StateBase> states, StateType initType)
    {
        States = states;
        CurrentType = initType;
        CurrentState = States[CurrentType];
        CurrentState?.OnEnterState();
    }

    public bool ChangeState(StateType newType)
    {
        if (CurrentType == newType && CurrentType != StateType.Hurt)
        {
            Debug.Log($"같은 상태 {CurrentType}, {newType}");
            return false;
        }

        if (!States[newType].CanExecute)
        {
            return false;
        }

        CurrentState?.OnExitState();
        CurrentType = newType;
        CurrentState = States[newType];
        CurrentState.OnEnterState();
        return true;
    }

    public void UpdateState()
    {
        if (CurrentState != null)
        {
            CurrentState.OnUpdateState();
        }
    }

}
