using UnityEngine;

namespace TowerDefence
{
    public enum TurretMode
    {
        Primary,
        Secondary,
        Auto
    }

    [CreateAssetMenu]
    /// <summary>
    /// ScriptableObject holding configuration values for a turret.
    /// </summary>
    public sealed class TurretProperties : ScriptableObject
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private Projectile m_ProjectilePrefab;
        public Projectile ProjectilePrefab => m_ProjectilePrefab;

        [SerializeField] private Sprite m_ProjectileSprite;
        public Sprite ProjectileSprite => m_ProjectileSprite;

        [SerializeField] private float m_RateOfFire;
        public float RateOfFire => m_RateOfFire;
        [SerializeField] private int m_Damage;
        public int Damage => m_Damage;

        public UpgradeAsset DamageUpgradeAsset;

        [SerializeField] private float m_ProjectileSpeed;
        public float ProjectileSpeed => m_ProjectileSpeed;

        [SerializeField] private AudioClip m_LaunchSFX;
        public AudioClip LaunchSFX => m_LaunchSFX;

        [SerializeField] private DamageType m_DamageType;
        public DamageType DamageType => m_DamageType;

        
    }
}