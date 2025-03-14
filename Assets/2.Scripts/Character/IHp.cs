using System;


public interface IHp
{
    public float CurrentHp { get; set; }
    public float MaxHp { get; set; }

    public event Action<float> OnHpChanged;
}
