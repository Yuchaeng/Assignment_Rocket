
public class StateNone : StateBase
{
    public StateNone(Monster monster) : base(monster)
    {
    }

    public override bool CanExecute => machine.CurrentType != StateType.Die;

    public override void OnEnterState()
    {
    }
    public override void OnUpdateState()
    {
    }

    public override void OnExitState()
    {
    }
}
