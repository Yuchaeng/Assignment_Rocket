using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 m_dir;
    private float m_CurTime = 0f;
    private float m_DurTime = 0.5f;
    private float m_Speed = 15f;

    public void SetDirection(Vector2 dir)
    {
        m_dir = dir;
    }

    private void Update()
    {
        if (m_CurTime < m_DurTime)
        {
            transform.position += (Vector3)m_dir.normalized * m_Speed * Time.deltaTime;
            m_CurTime += Time.deltaTime;
        }
        else
        {
            m_CurTime = 0f;
            ObjectPoolManager.Instance.ReleaseToPool(EPoolType.Bullet, gameObject);
        }
    }

}
