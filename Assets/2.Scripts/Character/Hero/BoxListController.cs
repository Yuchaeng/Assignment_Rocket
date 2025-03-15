using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxListController : MonoBehaviour
{
    [SerializeField] private List<Box> m_BoxList;
    [SerializeField] private Hero m_Hero;
    private int m_RemovedCount = 0;

    private Vector3 m_MoveDownPos;

    private void Start()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            m_BoxList.Add(transform.GetChild(i).GetComponent<Box>());
        }
        m_Hero = transform.GetChild(transform.childCount - 1).GetComponent<Hero>();

        float m_OffsetY = m_BoxList[1].transform.position.y - m_BoxList[0].transform.position.y;
        m_MoveDownPos = new Vector3(0, m_OffsetY);

        InitBoxes();
    }

    private void InitBoxes()
    {
        for (int i = 0; i < m_BoxList.Count; i++)
        {
            m_BoxList[i].MyIdx = i;
            m_BoxList[i].OnDeadEvent += MoveDown;
        }
    }

    /// <summary>
    /// 박스가 파괴될 때마다 아래로 이동합니다.
    /// </summary>
    /// <param name="removeIdx"></param>
    private void MoveDown(int removeIdx)
    {
        m_RemovedCount++;
        m_BoxList[removeIdx].OnDeadEvent -= MoveDown;
        for (int i = 0; i < m_BoxList.Count; i++)
        {
            if (m_BoxList[i] != null && i > removeIdx)
            {
                m_BoxList[i].transform.position -= m_MoveDownPos;
            }
        }
        m_Hero.transform.position -= m_MoveDownPos;

        // 박스가 다 파괴되었다면
        StartCoroutine(KillHero());
    }

    private IEnumerator KillHero()
    {
        if (m_RemovedCount == m_BoxList.Count)
        {
            float curret = 0f;
            float duration = 1f;

            while (curret < duration)
            {
                curret += Time.deltaTime;
                m_Hero.transform.rotation = Quaternion.Lerp(m_Hero.transform.rotation, Quaternion.Euler(0, 0, 90), curret / duration);
                yield return null;
            }

            yield return new WaitForSeconds(0.3f);
            Destroy(m_Hero.gameObject);
        }
    }

}
