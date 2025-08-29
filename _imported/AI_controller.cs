using UnityEngine;

namespace TowerDefence
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Range(0f, 1f)][SerializeField] private float m_NavigationLinear = 1f;
        [Range(0f, 1f)][SerializeField] private float m_NavigationAngular = 1f;
        [SerializeField] private float m_ArrivalDistance = 1.5f;
        [SerializeField] private float m_EvadeRayLength = 0f; // set >0 to enable simple evasion

        private SpaceShip m_SpaceShip;
        private Vector3 m_MovePosition;
        private bool m_HasTarget;

        private const float MAX_ANGLE = 45f;

        public float ArrivalDistance => m_ArrivalDistance;

        public void SetMoveTarget(Vector3 worldPos)
        {
            m_MovePosition = worldPos;
            m_HasTarget = true;
        }

        public bool ReachedTarget()
        {
            if (!m_HasTarget) return true;
            return Vector3.Distance(transform.position, m_MovePosition) <= m_ArrivalDistance;
        }

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();
        }

        private void Update()
        {
            if (!m_HasTarget) return;

            if (m_EvadeRayLength > 0f && Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength))
            {
                // very simple evade to the right
                m_MovePosition = transform.position + transform.right * 5f;
            }

            m_SpaceShip.ThrustControl = m_NavigationLinear;
            m_SpaceShip.TorqueControl = ComputeAlignTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
        }

        private static float ComputeAlignTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);
            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);
            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;
            return -angle;
        }
    }
}
