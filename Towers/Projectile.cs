using UnityEngine;

/// <summary>
/// Enum representing different types of damage that a projectile can inflict.
/// </summary>
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
        
        [SerializeField] private Sound m_HitSound;

        public DamageType Type
        {
            get => m_DamageType;
            set => m_DamageType = value;
        }

        /// <summary>
        /// Convenience setter to change projectile damage type at runtime.
        /// </summary>
        public void SetType(DamageType type)
        {
            m_DamageType = type;
        }

        /// <summary>
        /// Called when the projectile hits an enemy.
        /// </summary>
        /// <param name="destructible"></param>
        protected override void OnHit(Enemy destructible)
        {
            m_HitSound.Play();
            destructible?.TakeDamage(Damage, m_DamageType);
        }

        protected override void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            if (m_ImpactEffectPrefab != null)
                Instantiate(m_ImpactEffectPrefab, pos, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
