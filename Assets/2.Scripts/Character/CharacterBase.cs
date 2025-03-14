using System;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, IHp
{
    public float CurrentHp
    {
        get => m_CurrentHp;
        set
        {
            m_CurrentHp = value;
            OnHpChanged?.Invoke(m_CurrentHp);
        }
    }
    private float m_CurrentHp;
    public float MaxHp { get => m_MaxHp; set => m_MaxHp = value; }
    private float m_MaxHp;

    public event Action<float> OnHpChanged;

    public abstract void OnDamaged(float damage);

    public abstract void OnDead();
}
