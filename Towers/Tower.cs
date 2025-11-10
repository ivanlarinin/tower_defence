using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// Class representing a tower in the tower defense game.
    /// </summary>
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        [SerializeField] private float m_OriginUpOffset = 0.56f;
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
                Vector2 towerOrigin = (Vector2)transform.position + (Vector2)transform.up * m_OriginUpOffset;
                var enter = Physics2D.OverlapCircle(towerOrigin, m_Radius);
                if (enter != null)
                {
                    m_Target = enter.transform.root.GetComponent<Destructable>();
                }
            }

            if (m_Target != null)
            {
                Rigidbody2D targetRigidbody = m_Target.GetComponent<Rigidbody2D>();
                Vector2 targetPosition = m_Target.transform.position;

                if (targetRigidbody != null && m_Turrets != null && m_Turrets.Length > 0)
                {
                    Vector2 targetVelocity = targetRigidbody.linearVelocity;
                    float projectileSpeed = m_Turrets[0].ProjectileSpeed;
                    Vector2 towerOrigin = (Vector2)transform.position + (Vector2)transform.up * m_OriginUpOffset;
                    float timeToTarget = Vector2.Distance(towerOrigin, targetPosition) / projectileSpeed;

                    targetPosition += targetVelocity * timeToTarget;
                }

                Vector2 targetVector = targetPosition - ((Vector2)transform.position + (Vector2)transform.up * m_OriginUpOffset);
                float active_dist = targetVector.magnitude;

                if (active_dist <= m_Radius)
                {
                    foreach (var turret in m_Turrets)
                    {
                        Vector2 turretAim = targetPosition - (Vector2)turret.transform.position;
                        turret.transform.up = turretAim.normalized;
                        turret.Fire();
                        #if UNITY_EDITOR
                        Debug.DrawLine(turret.transform.position, targetPosition, Color.green);
                        #endif
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