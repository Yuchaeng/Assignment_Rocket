using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("스폰 위치")]
    [SerializeField] private Transform m_Line1SpawnPos; 
    [SerializeField] private Transform m_Line2SpawnPos; 
    [SerializeField] private Transform m_Line3SpawnPos;

    [Header("몬스터 프리팹")]
    [SerializeField] private GameObject m_Line1MonsterPrefab;
    [SerializeField] private GameObject m_Line2MonsterPrefab;
    [SerializeField] private GameObject m_Line3MonsterPrefab;

    [Header("몬스터 Head, Body Sprite")]
    [SerializeField] private Sprite[] m_HeadSprites;
    [SerializeField] private Sprite[] m_BodySprites;

    private float m_SpawnTime;

    [Header("체력바 프리팹")]
    [SerializeField] private GameObject m_HpBar;
    [Header("체력바용 캔버스")]
    [SerializeField] private Transform m_Canvas;


    private void Start()
    {
        CreatePools();
        StartCoroutine(SpawnMonsterLine1());
        StartCoroutine(SpawnMonsterLine2());
        StartCoroutine(SpawnMonsterLine3());
    }

    private void CreatePools()
    {
        ObjectPoolManager.Instance.CreatePools(EPoolType.Monster1, m_Line1MonsterPrefab, 15);
        ObjectPoolManager.Instance.CreatePools(EPoolType.Monster2, m_Line2MonsterPrefab, 15);
        ObjectPoolManager.Instance.CreatePools(EPoolType.Monster3, m_Line3MonsterPrefab, 15);
        ObjectPoolManager.Instance.CreatePools(EPoolType.HpBar, m_HpBar, 30);
    }

    /// <summary>
    /// 몬스터를 생성합니다.
    /// </summary>
    /// <param name="type">풀 타입</param>
    /// <param name="spawnPos">생성할 위치</param>
    private void SpawnMonster(EPoolType type, Transform spawnPos)
    {
        GameObject monster = ObjectPoolManager.Instance.GetFromPool(type);
        Monster monsterComp = monster.GetComponent<Monster>();

        // 몬스터 외모 변경
        int idx = Random.Range(0, m_HeadSprites.Length);
        monsterComp.ChangeAppearance(m_HeadSprites[idx], m_BodySprites[idx]);
        monster.transform.position = spawnPos.position;

        InitHpBar(monsterComp);
    }

    /// <summary>
    /// 몬스터가 가질 hp바를 생성합니다.
    /// </summary>
    /// <param name="monsterComp"></param>
    private void InitHpBar(Monster monsterComp)
    {
        GameObject bar = ObjectPoolManager.Instance.GetFromPool(EPoolType.HpBar);
        bar.transform.SetParent(m_Canvas);
        bar.SetActive(false);
        monsterComp.SetHpBar(bar);
    }

    private IEnumerator SpawnMonsterLine1()
    {
        while (true)
        {
            m_SpawnTime = Random.Range(1f, 5f);
            yield return new WaitForSeconds(m_SpawnTime);

            SpawnMonster(EPoolType.Monster1, m_Line1SpawnPos);
        }
    }

    private IEnumerator SpawnMonsterLine2()
    {
        while (true)
        {
            m_SpawnTime = Random.Range(1.5f, 7f);
            yield return new WaitForSeconds(m_SpawnTime);

            SpawnMonster(EPoolType.Monster2, m_Line2SpawnPos);
        }
    }

    private IEnumerator SpawnMonsterLine3()
    {
        while (true)
        {
            m_SpawnTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(m_SpawnTime);

            SpawnMonster(EPoolType.Monster3, m_Line3SpawnPos);
        }
    }

}
