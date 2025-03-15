using System;
using System.Collections;
using UnityEngine;


public class Hero : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private GameObject m_Gun;
    [SerializeField] private GameObject m_Bullet;

    private float m_ViewRadius = 10f;
    private int m_MonsterLayer1;
    private int m_MonsterLayer2;
    private int m_MonsterLayer3;

    private GameObject m_Target;
    private float m_AttackSpeed = 3;
    private WaitForSeconds m_WaitForAttack;
    private float m_DamageToMonster = 5f;


    private void Start()
    {
        m_MonsterLayer1 = 1 << LayerMask.NameToLayer("MonsterLine1");
        m_MonsterLayer2 = 1 << LayerMask.NameToLayer("MonsterLine2");
        m_MonsterLayer3 = 1 << LayerMask.NameToLayer("MonsterLine3");

        m_WaitForAttack = new WaitForSeconds(1 / m_AttackSpeed);

        ObjectPoolManager.Instance.CreatePools(EPoolType.Bullet, m_Bullet, 5);
        StartCoroutine(StartFindAndAttack());
    }

    private IEnumerator StartFindAndAttack()
    {
        while (true)
        {
            yield return m_WaitForAttack;
            FindAndAttackMonster();
        } 
    }

    private void FindAndAttackMonster()
    {
        Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(transform.position, m_ViewRadius, m_MonsterLayer1 | m_MonsterLayer2 | m_MonsterLayer3);
        float minDistance = 999f;

        foreach (Collider2D hit in hitMonsters)
        {
            Vector2 dir = (hit.transform.position - transform.position).normalized;

            float dotDown = Vector2.Dot(-transform.up, dir);
            float dotRight = Vector2.Dot(transform.right, dir);
            
            // 범위 내에서 가장 가까운 몬스터를 공격
            if (dotDown >= 0 && dotRight >= 0)
            {
                float curDistance = Vector2.Distance(hit.transform.position, transform.position);
                if (curDistance < minDistance)
                {
                    minDistance = curDistance;
                    m_Target = hit.gameObject;
                }
            }
        }
        

        if (m_Target != null && m_Target.TryGetComponent(out CharacterBase target))
        {
            // 총 방향 회전
            Vector2 dir = m_Target.transform.position - m_Gun.transform.position;
            float rotZ = MathF.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            m_Gun.transform.rotation = Quaternion.Euler(0, 0, rotZ);

            // 총알 발사
            GameObject bullet = ObjectPoolManager.Instance.GetFromPool(EPoolType.Bullet);
            bullet.transform.position = m_Gun.transform.position;
            bullet.GetComponent<Bullet>().SetDirection(dir);
            target.OnDamaged(m_DamageToMonster);
        }
    }
}
