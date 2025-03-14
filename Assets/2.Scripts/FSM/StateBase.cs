using UnityEngine;

public enum StateType
{
    Default,
    Jump,
    MoveBackward,
    Attack,
    Hurt,
    Die,
}

public abstract class StateBase
{
    protected Monster monster;
    protected Rigidbody2D rigid;
    protected Collider2D collider;
    protected Animator animator;
    protected StateMachine machine;

    public StateBase(Monster monster)
    {
        this.monster = monster;
        rigid = monster.GetComponent<Rigidbody2D>();
        collider = monster.GetComponent<CapsuleCollider2D>();
        animator = monster.GetComponent<Animator>();
        machine = monster.StateMachine;
    }

    public abstract bool CanExecute { get; }
    public abstract void OnEnterState(object obj = null);
    public abstract void OnUpdateState();
    public abstract void OnExitState();
}
