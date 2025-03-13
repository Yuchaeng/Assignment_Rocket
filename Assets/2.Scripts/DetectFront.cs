using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectFront : MonoBehaviour
{
    private Rigidbody2D m_Rigid;
    private Monster m_Monster;

    private void Start()
    {
        m_Monster = transform.parent.GetComponent<Monster>();
        m_Rigid = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Hero"))
        {
            if (Mathf.Abs(m_Rigid.velocity.y) < 0.1f)
            {
                Debug.LogWarning("끼임" + m_Monster.gameObject.name);
                m_Rigid.velocity = Vector2.down * 5;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Hero"))
        {
            if (Mathf.Abs(m_Rigid.velocity.y) < 0.1f)
            {
                Debug.LogWarning("끼임" + m_Monster.gameObject.name);
                m_Rigid.velocity = Vector2.down * 5;
            }
        }
    }
}
