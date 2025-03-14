using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum EPoolType
{
    None,
    Monster1,
    Monster2,
    Monster3,
    HpBar,
    Bullet,
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;
    private Dictionary<EPoolType, IObjectPool<GameObject>> pools = new();

    private Transform m_MonsterContainer;
    private Transform m_HpBarContainer;
    private Transform m_BulletContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitObjectPoolManager();
    }

    /// <summary>
    /// 오브젝트풀 초기화
    /// </summary>
    public void InitObjectPoolManager()
    {
        m_MonsterContainer = new GameObject("MonsterContainer").transform;
        m_HpBarContainer = new GameObject("HpBarContainer").transform;
        m_BulletContainer = new GameObject("BulletContainer").transform;

        m_MonsterContainer.SetParent(transform);
        m_HpBarContainer.SetParent(transform);
        m_BulletContainer.SetParent(transform);
    }

    /// <summary>
    /// 오브젝트 풀 생성
    /// </summary>
    /// <param name="type">풀 타입</param>
    /// <param name="prefab">생성할 프리팹</param>
    /// <param name="maxSize">생성할 개수</param>
    public void CreatePools(EPoolType type, GameObject prefab, int maxSize)
    {
        IObjectPool<GameObject> newPool = new ObjectPool<GameObject>(
            createFunc: () => {
                return type switch
                {
                    EPoolType.Monster1 => Instantiate(prefab, m_MonsterContainer),
                    EPoolType.Monster2 => Instantiate(prefab, m_MonsterContainer),
                    EPoolType.Monster3 => Instantiate(prefab, m_MonsterContainer),
                    EPoolType.HpBar => Instantiate(prefab, m_HpBarContainer),
                    EPoolType.Bullet => Instantiate(prefab, m_BulletContainer),
                    _ => null,
                };
            },
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: Destroy,
            maxSize: maxSize
        );

        pools.Add(type, newPool);
    }

    /// <summary>
    /// 오브젝트 풀에서 객체를 꺼냅니다.
    /// </summary>
    /// <param name="poolType">꺼낼 풀의 타입</param>
    /// <returns></returns>
    public GameObject GetFromPool(EPoolType poolType)
    {
        if (!pools.ContainsKey(poolType))
        {
            Debug.LogError($"딕셔너리에 {poolType}풀이 없음");
            return null;
        }

        return pools[poolType].Get();
    }

    /// <summary>
    /// 객체를 풀로 반환합니다.
    /// </summary>
    /// <param name="poolType">반환되어 들어갈 풀의 타입</param>
    /// <param name="obj">반환할 객체</param>
    public void ReleaseToPool(EPoolType poolType, GameObject obj)
    {
        if (!pools.ContainsKey(poolType))
        {
            Debug.LogError($"딕셔너리에 {poolType}풀이 없음");
            return;
        }

        pools[poolType].Release(obj);
    }
}
