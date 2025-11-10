using UnityEngine;

namespace TowerDefence
{
    [DisallowMultipleComponent]
    /// <summary>
    /// Turret controller that handles firing logic and refire timing.
    /// </summary>
    public class Turret : MonoBehaviour
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;

        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;

        /// <summary>
        /// Gets the projectile speed from the turret properties.
        /// </summary>
        public float ProjectileSpeed => m_TurretProperties != null ? m_TurretProperties.ProjectileSpeed : 0;

        private void Start()
        {
        }

        private void Update()
        {
            if (m_RefireTimer > 0)
            {
                m_RefireTimer -= Time.deltaTime;
            }
            else if (Mode == TurretMode.Auto)
            {

            }
        }

        /// <summary>
        /// Fires the turret if ready, spawning and configuring a projectile.
        /// </summary>
        public void Fire()
        {
            if (m_TurretProperties == null) return;
            if (m_RefireTimer > 0) return;

            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab);
            projectile.GetComponentInChildren<SpriteRenderer>().sprite = m_TurretProperties.ProjectileSprite;
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            projectile.SetVelocity(ProjectileSpeed);
            projectile.SetType(m_TurretProperties.DamageType);
            projectile.Damage = m_TurretProperties.Damage;

            int damageBonus = 0;
            if (Upgrades.Instance != null)
            {
                var damageUpgradeLevel = Upgrades.GetUpgradeLevel(m_TurretProperties.DamageUpgradeAsset);
                damageBonus = damageUpgradeLevel;
            }
            projectile.Damage += damageBonus;

            m_RefireTimer = m_TurretProperties.RateOfFire;

            Sound.Arrow.Play();
        }

        /// <summary>
        /// Assigns turret properties at runtime.
        /// </summary>
        public void SetTurretProperties(TurretProperties properties)
        {
            m_TurretProperties = properties;
        }
    }
}