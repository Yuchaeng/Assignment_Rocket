using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectUpper : MonoBehaviour
{
    private Monster m_Monster;

    private void Start()
    {
        m_Monster = GetComponentInParent<Monster>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == gameObject.layer)
        {
            m_Monster.StateMachine.ChangeState(StateType.MoveBackward);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == gameObject.layer)
        {
            if (!m_Monster.IsMovingBackward)
            {
                m_Monster.StateMachine.ChangeState(StateType.MoveBackward);
            }
        }
    }

}
