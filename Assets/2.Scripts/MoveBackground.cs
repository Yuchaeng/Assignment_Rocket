using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    private float m_MoveDistance = -0.7f;

    private void Update()
    {
        transform.position += new Vector3(m_MoveDistance, 0, 0) * Time.deltaTime;
    }
}
