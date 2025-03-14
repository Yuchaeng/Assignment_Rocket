using System.Collections.Generic;

public class StateMachine
{
    public StateType CurrentType;
    public StateBase CurrentState;
    public Dictionary<StateType, StateBase> States;

    public StateMachine()
    {
    }

    /// <summary>
    /// 상태를 정의하고 초기화합니다.
    /// </summary>
    /// <param name="states">가질 상태들 넣은 딕셔너리</param>
    /// <param name="initType">초기 상태</param>
    public void Init(Dictionary<StateType, StateBase> states, StateType initType)
    {
        States = states;
        CurrentType = initType;
        CurrentState = States[CurrentType];
        CurrentState?.OnEnterState();
    }

    public bool ChangeState(StateType newType, object obj = null)
    {
        if (CurrentType == newType)
        {
            return false;
        }

        if (!States[newType].CanExecute)
        {
            return false;
        }

        CurrentState?.OnExitState();
        CurrentType = newType;
        CurrentState = States[newType];
        CurrentState.OnEnterState(obj);
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
