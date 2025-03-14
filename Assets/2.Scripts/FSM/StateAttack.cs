using UnityEngine;

public class StateAttack : StateBase
{
    private float m_DamageToHero = 3f;
    private RaycastHit2D m_Hit;

    public StateAttack(Monster monster) : base(monster)
    {
    }

    public override bool CanExecute => true;

    public override void OnEnterState(object obj = null)
    {
        monster.CanMove = true;

        m_Hit = (RaycastHit2D)obj;
        if (m_Hit.transform.TryGetComponent(out CharacterBase box))
        {
            box.OnDamaged(m_DamageToHero);
        }

        animator.SetTrigger("IsAttack");
        
    }
    public override void OnUpdateState()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
        {
            machine.ChangeState(StateType.Default);
        }
    }

    public override void OnExitState()
    {
    }
}
