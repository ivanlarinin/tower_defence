using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// 2D character motor for top-down movement with acceleration, deceleration, and turning.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMotor : Destructable
    {
        [Header("Move")]
        [SerializeField] float m_MaxSpeed = 5f;
        public float maxSpeed
        {
            private get => m_MaxSpeed;
            set => m_MaxSpeed = value;
        }
        
        [SerializeField] float m_Acceleration = 30f;
        [SerializeField] float m_Deceleration = 40f;

        [Header("Turn")]
        [SerializeField] float m_TurnSpeedDeg = 720f;

        Rigidbody2D rb;
        Vector2 desiredDir;
        bool haveDesired;
        float desiredSpeed;
        float targetAngleDeg;
        bool haveTargetAngle;

        public void SetDesiredMove(Vector2 dir01, float speed01)
        {
            desiredDir = dir01.normalized;
            desiredSpeed = Mathf.Clamp01(speed01) * m_MaxSpeed;
            haveDesired = speed01 > 0.0001f;
        }

        public void SetDesiredFacing(Vector2 worldPoint)
        {
            Vector2 toT = worldPoint - rb.position;
            if (toT.sqrMagnitude < 0.0001f) { haveTargetAngle = false; return; }
            targetAngleDeg = Mathf.Atan2(toT.y, toT.x) * Mathf.Rad2Deg - 90f;
            haveTargetAngle = true;
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.linearDamping = 0f;
            rb.angularDamping = 0f;
        }

        void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            if (haveTargetAngle)
            {
                float newAngle = Mathf.MoveTowardsAngle(rb.rotation, targetAngleDeg, m_TurnSpeedDeg * dt);
                rb.MoveRotation(newAngle);
            }

            // --- Velocity (accelerate/decelerate toward desired)
            Vector2 vel = rb.linearVelocity;
            float targetSpeed = haveDesired ? desiredSpeed : 0f;
            Vector2 targetVel = haveDesired ? desiredDir * targetSpeed : Vector2.zero;

            float accel = (targetSpeed > vel.magnitude) ? m_Acceleration : m_Deceleration;
            Vector2 newVel = Vector2.MoveTowards(vel, targetVel, accel * dt);
            rb.linearVelocity = newVel;

            haveDesired = false;
            haveTargetAngle = false;
        }
    }
}
