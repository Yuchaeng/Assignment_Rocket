using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    [SerializeField] private Transform[] m_Backgrounds;
    private float m_Speed = 1.2f;
    private float m_Offset;
    private float m_MovingPosX;

    private float m_ScreenWidthSize;
    private float m_ScreenHeightHalfSize;

    private void Start()
    {
        m_Offset = (m_Backgrounds[1].position.x - m_Backgrounds[0].position.x) * 2;

        m_ScreenHeightHalfSize = Camera.main.orthographicSize;
        m_ScreenWidthSize = m_ScreenHeightHalfSize * Camera.main.aspect * 2;
        m_MovingPosX = m_Backgrounds[0].position.x + m_ScreenWidthSize;
    }

    private void Update()
    {
        for (int i = 0; i < m_Backgrounds.Length; i++)
        {
            m_Backgrounds[i].transform.position += m_Speed * Vector3.left * Time.deltaTime;

            if (m_Backgrounds[i].position.x < -m_MovingPosX)
            {
                m_Backgrounds[i].position += new Vector3(m_Offset, 0);
            }
        }
    }

}
