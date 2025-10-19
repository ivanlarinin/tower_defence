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
                Rigidbody2D targetRigidbody = m_Target.GetComponent<Rigidbody2D>();
                Vector2 targetPosition = m_Target.transform.position;

                if (targetRigidbody != null)
                {
                    Vector2 targetVelocity = targetRigidbody.linearVelocity;
                    float projectileSpeed = m_Turrets[0].ProjectileSpeed;
                    float timeToTarget = Vector2.Distance(transform.position, targetPosition) / (projectileSpeed * 1.1f);

                    // Predict the target's future position
                    targetPosition += targetVelocity * timeToTarget;
                }

                Vector2 targetVector = targetPosition - (Vector2)transform.position;
                float active_dist = targetVector.magnitude;

                if (active_dist <= m_Radius)
                {
                    foreach (var turret in m_Turrets)
                    {
                        turret.transform.up = targetVector.normalized;
                        turret.Fire();
                        Debug.DrawLine(turret.transform.position, targetPosition, Color.red);
                    }
                }
            }
        }

        public void Use(TowerAssets assets)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = assets.sprite;
            m_Turrets = GetComponentsInChildren<Turret>();
            foreach (var turret in m_Turrets)
            {
                // turret.AssignLoadout(assets.turretProperties);
            }
            GetComponentInChildren<BuildSite>().SetBuildableTowers(assets.m_UpgradesTo);
        }
    }
}