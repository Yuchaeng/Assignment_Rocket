using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum StateType
{
    None,
    Move,
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
    protected StateMachine machine;

    public StateBase(Monster monster)
    {
        this.monster = monster;
        rigid = monster.GetComponent<Rigidbody2D>();
        collider = monster.GetComponent<CapsuleCollider2D>();
        machine = monster.StateMachine;
    }

    public abstract bool CanExecute { get; }
    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnExitState();
}
