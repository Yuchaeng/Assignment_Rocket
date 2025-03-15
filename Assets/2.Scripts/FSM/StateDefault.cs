
public class StateDefault : StateBase
{
    public StateDefault(Monster monster) : base(monster)
    {
    }

    public override bool CanExecute => machine.CurrentType != StateType.Die;

    public override void OnEnterState(object obj = null)
    {
        monster.CanMove = true;
    }
    public override void OnUpdateState()
    {
    }

    public override void OnExitState()
    {
    }
}
