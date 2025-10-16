using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// AI controller that moves towards a target position
    /// </summary>
    [RequireComponent(typeof(Enemy))]
    public class AIController : MonoBehaviour
    {
        [Header("Nav")]
        [SerializeField] float m_ArrivalDistance = 1.0f;
        [SerializeField] float m_Speed = 1.0f;

        public float Speed
        {
            private get => m_Speed;
            set => m_Speed = value;
        }

        CharacterMotor motor;
        Vector3 m_MovePosition;
        bool m_HasTarget;

        public float ArrivalDistance => m_ArrivalDistance;
        public void SetMoveTarget(Vector3 worldPos) { m_MovePosition = worldPos; m_HasTarget = true; }
        public bool ReachedTarget() => !m_HasTarget || Vector3.Distance(transform.position, m_MovePosition) <= m_ArrivalDistance;

        void Awake() => motor = GetComponent<CharacterMotor>();

        void FixedUpdate()
        {
            if (!m_HasTarget) return;

            Vector2 toT = m_MovePosition - transform.position;
            float dist = toT.magnitude;

            motor.SetDesiredFacing(m_MovePosition);

            if (dist > m_ArrivalDistance)
            {
                motor.SetDesiredMove(toT / Mathf.Max(dist, 0.0001f), m_Speed);
            }
        }
    }
}
