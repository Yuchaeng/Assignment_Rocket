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
        if (collision.gameObject.layer == gameObject.layer)
        {
            
            //Debug.Log("충돌!!!!!!!!!!!!!!!!");

            //m_Monster.StateMachine.ChangeState(StateType.Jump);
            //m_Rigid.velocity = Vector2.zero;

            //Vector2 direction = new Vector2(-1f, 1.5f);
            //m_Rigid.AddForce(direction * 3f, ForceMode2D.Impulse);

            //m_Monster.CanMove = true;

            // 덜커덩하지만 원하는 곳에 착지함
            //m_Rigid.AddForce(Vector2.up * 1.5f, ForceMode2D.Impulse);
            //transform.parent.DOJump(m_Rigid.position + new Vector2(-0.5f, 0), 1f, 1, 0.2f, false);
        }
    }
}
