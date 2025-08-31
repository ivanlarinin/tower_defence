using UnityEngine;

namespace TowerDefence
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;

        private Turret[] m_Turrets;
        private Destructable m_Target = null; 

#if UNITY_EDITOR
        private static readonly Color GizmoColor = new Color(1, 0, 0, 0.3f);

        private void OnDrawGizmosSelected()
        {
            // Draw tower active zone
            Gizmos.color = GizmoColor;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }
#endif

        private void Start()
        {
            m_Turrets = GetComponentsInChildren<Turret>();
        }

        private void Update()
        {
            if (m_Turrets != null)
            {
                var enter = Physics2D.OverlapCircle(transform.position, m_Radius);
                if (enter != null)
                {
                    m_Target = enter.transform.root.GetComponent<Destructable>();
                }
            }

            if (m_Target != null)
            {
                // Debug.Log("Firing at target: " + m_Target.name);
                Vector2 targetVector = m_Target.transform.position - transform.position;
                float active_dist = targetVector.magnitude;

                if (active_dist <= m_Radius)
                {
                    foreach (var turret in m_Turrets)
                    {
                        turret.transform.up = targetVector.normalized;
                        turret.Fire();
                        Debug.DrawLine(turret.transform.position, m_Target.transform.position, Color.red);
                    }
                }
            }
        }

    }

}

