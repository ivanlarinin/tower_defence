using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private List<Transform> m_Points = new List<Transform>();

        public int Count
        {
            get
            {
                if (m_Points != null)
                    return m_Points.Count;
                else
                    return 0;
            }
        }

        public Transform this[int i] => m_Points[i];

        public Vector3 GetPosition(int i) => m_Points[i] != null ? m_Points[i].position : transform.position;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (m_Points == null || m_Points.Count == 0) return;

            Gizmos.color = Color.green;
            for (int i = 0; i < m_Points.Count; i++)
            {
                if (!m_Points[i]) continue;
                Gizmos.DrawWireSphere(m_Points[i].position, 0.4f);
                if (i < m_Points.Count - 1 && m_Points[i + 1])
                {
                    Gizmos.DrawLine(m_Points[i].position, m_Points[i + 1].position);
                }
            }
        }
#endif
    }
}
