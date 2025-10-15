using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// Base class for all projectile types.
    /// Handles movement, raycast hit detection, and lifetime.
    /// Inherit from this class to implement specific projectile behavior (effects, scoring, etc.).
    /// </summary>
    public abstract class ProjectileBase : Entity
    {
        [SerializeField] private float m_Velocity;
        [SerializeField] private float m_Lifetime;
        [SerializeField] private int m_Damage;

        private float m_Timer;
        protected Destructable m_Parent;

        /// <summary>
        /// Damage value of the projectile
        /// </summary>
        public int Damage
        {
            get => m_Damage;
            set => m_Damage = value;
        }

        /// <summary>
        /// Set the parent/destructable that fired this projectile
        /// </summary>
        public void SetParentShooter(Destructable parent)
        {
            m_Parent = parent;
        }

        /// <summary>
        /// Called when the projectile hits a destructible object
        /// Override this to implement custom hit logic
        /// </summary>
        protected virtual void OnHit(Enemy destructible) { }

        /// <summary>
        /// Called when the projectile collides with any 2D collider
        /// Override to implement custom collision behavior (particles, sounds)
        /// </summary>
        protected virtual void OnCollide2D(Collider2D collider) { }

        /// <summary>
        /// Called when the projectile's lifetime ends or it hits something
        /// Override to implement effects or destruction logic
        /// </summary>
        protected virtual void OnProjectileLifeEnd(Collider2D collider, Vector2 hitPosition) { }

        private void Update()
        {
            float stepLength = Time.deltaTime * m_Velocity;
            Vector2 step = transform.up * stepLength;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);
            Collider2D hitCollider = hit ? hit.collider : null;
            Vector2 hitPos = hit ? hit.point : (Vector2)transform.position;

            if (hitCollider != null)
            {
                OnCollide2D(hitCollider);

                if (hitCollider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    OnHit(enemy);
                    OnProjectileLifeEnd(hitCollider, hitPos);
                    return;
                }
            }

            m_Timer += Time.deltaTime;

            if (m_Timer > m_Lifetime)
            {
                OnProjectileLifeEnd(hitCollider, hitPos);
            }

            transform.position += new Vector3(step.x, step.y, 0);
        }
    }
}
