using System;
using UnityEngine;


public class Box : CharacterBase
{
    [SerializeField] private BoxHpBar m_HpBar;
    private bool m_IsFirstDamage = true;
    public int MyIdx { get; set; }
    public event Action<int> OnDeadEvent;

    private void Start()
    {
        MaxHp = 100;
        CurrentHp = 100;
    }

    public override void OnDamaged(float damage)
    {
        if (m_IsFirstDamage)
        {
            m_IsFirstDamage = false;
            m_HpBar.GetSliderComp();
            m_HpBar.SetActiveBar();
            m_HpBar.InitHpBar(this);
        }

        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            OnDead();
        }
    }

    public override void OnDead()
    {
        OnDeadEvent?.Invoke(MyIdx);
        Destroy(gameObject);
    }
}