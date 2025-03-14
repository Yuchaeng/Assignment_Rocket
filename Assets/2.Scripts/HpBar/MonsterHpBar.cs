using UnityEngine;

public class MonsterHpBar : HpBar<Monster>
{
    private Vector3 m_hpPos;

    private void OnEnable()
    {
        m_hpPos = new Vector3(-0.3f, 1.2f);
    }

    private void Update()
    {
        transform.position = owner.transform.position + m_hpPos;
    }
}
