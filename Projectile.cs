using UnityEngine;

namespace TowerDefence
{
    public enum DamageType
    {
        Physical,
        Magic
    }

    public class Projectile : ProjectileBase
    {
        [SerializeField] private ImpactEffect m_ImpactEffectPrefab;
        [SerializeField] private DamageType m_DamageType;

        public DamageType Type
        {
            get => m_DamageType;
            set => m_DamageType = value;
        }

        /// <summary>
        /// Called when the projectile hits a destructible object
        /// </summary>
        protected override void OnHit(Enemy destructible)
        {
            if (destructible != null)
            {
                destructible.TakeDamage(Damage, m_DamageType);
            }
        }


        /// <summary>
        /// Called when the projectile's lifetime ends or it hits something
        /// </summary>
        protected override void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            if (m_ImpactEffectPrefab != null)
            {
                Instantiate(m_ImpactEffectPrefab, pos, Quaternion.identity);
            }

            Destroy(gameObject);
        }

        private void OnCollideWithCollider(Collider2D collider)
        {
            OnCollide2D(collider);

            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                OnHit(enemy);
                OnProjectileLifeEnd(collider, collider.transform.position);
            }
        }

        protected override void OnCollide2D(Collider2D collider)
        {
            // Optional: add sound, particles, etc.
        }
    }
}
