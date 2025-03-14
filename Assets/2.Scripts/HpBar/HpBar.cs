using UnityEngine;
using UnityEngine.UI;

public class HpBar<T> : MonoBehaviour
    where T : CharacterBase
{
    protected Slider hpBar;
    protected T owner;

    public void GetSliderComp()
    {
        hpBar = GetComponent<Slider>();
    }

    public void InitHpBar(T barOwner)
    {
        owner = barOwner;
        hpBar.maxValue = owner.MaxHp;
        hpBar.value = owner.CurrentHp;
        owner.OnHpChanged += ChangeHp;
    }

    public void RemoveHpEvent()
    {
        owner.OnHpChanged -= ChangeHp;
    }

    private void ChangeHp(float hp)
    {
        hpBar.value = hp;
    }

    public void SetActiveBar()
    {
        gameObject.SetActive(true);
    }
}
