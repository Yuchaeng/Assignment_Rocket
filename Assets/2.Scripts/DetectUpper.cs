using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectUpper : MonoBehaviour
{
    private Rigidbody2D m_Rigid;
    private Monster m_Monster;

    private void Start()
    {
        m_Monster = transform.parent.GetComponent<Monster>();
        m_Rigid = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == gameObject.layer)
        {
            //m_Monster.m_CanMove = false;
            //m_Rigid.velocity = new Vector2(2f, 0);
            m_Monster.StateMachine.ChangeState(StateType.MoveBackward);
            Debug.Log("위!!!!!!!!!!!!!!!!");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == gameObject.layer)
        {
            Debug.Log("뒤로가는중");
            if (!m_Monster.IsMovingBackward)
            {
                m_Monster.StateMachine.ChangeState(StateType.MoveBackward);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("끝");
        //m_Monster.m_CanMove = true;
        //m_Monster.StateMachine.ChangeState(StateType.None);

    }

}
