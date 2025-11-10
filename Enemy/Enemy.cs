using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// Represents an enemy in the game, including its stats, behavior, and interactions with the player.
    /// </summary>
    [DisallowMultipleComponent]
    [SelectionBase]
    [RequireComponent(typeof(AIController))]
    [RequireComponent(typeof(Enemy))]
    public class Enemy : Destructable
    {
        #region Fields and Properties

        [SerializeField] private SpriteRenderer m_SpriteRenderer;

        protected override SpriteRenderer GetSpriteRenderer()
        {
            return m_SpriteRenderer;
        }

        [SerializeField] private Animator m_Animator;
        [SerializeField] private CircleCollider2D m_CircleCollider;
        [SerializeField] private Transform m_Scale;
        [SerializeField] private int m_Damage = 1;
        [SerializeField] private int m_Armor = 1;
        [SerializeField] private int m_Gold = 1;
        private AIController m_AIController;
        private CharacterMotor m_CharacterMotor;

        // Buff-related fields
        [SerializeField] private GameObject m_BuffFX;
        [SerializeField] private float m_BuffRadius = 2f;
        [SerializeField] private float m_BuffCooldown = 4f;
        private float m_LastBuffTime = -Mathf.Infinity;
        private int m_Buff = 0;

        [SerializeField] private ArmorType m_ArmorType;

        public enum ArmorType { Base = 0, Mage = 1 }

        private static readonly Func<int, DamageType, int, int>[] ArmorDamageFunctions =
        {
            // Base armor reduces Physical damage only
            (damage, damageType, armor) =>
                    damageType == DamageType.Physical ? Math.Max(0, damage - armor) : damage,

            // Magic armor reduces ALL damage types
            (damage, damageType, armor) =>
                Math.Max(0, damage - armor)
        };

        private Destructable m_Destructable;

        #endregion

        #region Unity Methods

        /// <summary>
        /// Caches references to required components.
        /// </summary>
        private void Awake()
        {
            CacheRefs();
            DeathEvent += GiveGold;
        }


        /// <summary>
        /// Validates and caches references in the editor.
        /// </summary>
        private void OnValidate()
        {
            if (Application.isPlaying) return;
            CacheRefs();
        }

        /// <summary>
        /// Updates the enemy's behavior, including buffing nearby enemies if applicable.
        /// </summary>
        private void Update()
        {
            if (m_Buff == 1)
                TryBuffNearbyEnemy();
        }

        #endregion

        #region Buff Logic

        /// <summary>
        /// Attempts to buff nearby enemies within the buff radius.
        /// </summary>
        private void TryBuffNearbyEnemy()
        {
            if (Time.time - m_LastBuffTime < m_BuffCooldown)
                return;

            m_LastBuffTime = Time.time;

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, m_BuffRadius);
            foreach (var hit in hits)
            {
                if (hit.gameObject == this.gameObject) continue;

                CharacterMotor other = hit.GetComponentInParent<CharacterMotor>();
                if (other != null)
                {
                    AddHitPoints(10);
                    if (m_BuffFX != null)
                    {
                        Instantiate(m_BuffFX, other.transform.position, Quaternion.identity);
                    }
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Caches references to required components.
        /// </summary>
        private void CacheRefs()
        {
            if (!m_SpriteRenderer) m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
            if (!m_Animator) m_Animator = GetComponentInChildren<Animator>(true);
            if (!m_CircleCollider) m_CircleCollider = GetComponentInChildren<CircleCollider2D>(true);

            if (!m_AIController) TryGetComponent(out m_AIController);
            if (!m_CharacterMotor) TryGetComponent(out m_CharacterMotor);

            if (!m_Scale && m_SpriteRenderer)
                m_Scale = m_SpriteRenderer.transform;

            m_Destructable = GetComponent<Destructable>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Configures the enemy using the specified asset.
        /// </summary>
        /// <param name="asset">The asset containing enemy configuration data.</param>
        public void Use(EnemyAsset asset)
        {
            CacheRefs();

            if (m_SpriteRenderer) m_SpriteRenderer.color = asset.color;

            if (m_Animator && asset.animations)
                m_Animator.runtimeAnimatorController = asset.animations;

            if (m_AIController)
                m_AIController.Speed = asset.Speed;

            if (m_CharacterMotor)
                m_CharacterMotor.maxSpeed = asset.Speed;

            if (m_CircleCollider)
                m_CircleCollider.radius = asset.Radius;

            if (m_Scale)
            {
                float scale = asset.Radius * 4f;
                m_Scale.localScale = new Vector3(scale, scale, 1f);
            }

            m_Damage = asset.damage;
            m_Gold = asset.gold;
            m_Buff = asset.buff;
            m_Armor = asset.armor;
            m_ArmorType = asset.armorType;
        }

        /// <summary>
        /// Damages the player with the enemy's damage value.
        /// </summary>
        public void DamagePlayer()
        {
            // TODO: Damage the player with m_Damage
        }

        /// <summary>
        /// Awards gold to the player when the enemy is killed.
        /// </summary>
        public void GiveGold()
        {
            TDPlayer player = TDPlayer.Instance;
            if (player != null)
            {
                player.ChangeGold(m_Gold);
            }
        }

        /// <summary>
        /// Applies damage to the enemy, considering its armor type and damage type.
        /// </summary>
        /// <param name="damage">The amount of damage to apply.</param>
        /// <param name="damageType">The type of damage being applied.</param>
        public void TakeDamage(int damage, DamageType damageType)
        {
            if (m_Destructable != null)
            {
                int finalDamage = ArmorDamageFunctions[(int)m_ArmorType](damage, damageType, m_Armor);
                m_Armor -= damage;
                m_Destructable.ApplyDamage(finalDamage, damageType);
            }
        }

        #endregion
    }

#if UNITY_EDITOR
    /// <summary>
    /// Custom inspector for the Enemy class, allowing quick application of EnemyAsset configurations.
    /// </summary>
    [CustomEditor(typeof(Enemy))]
    public class EnemyInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EnemyAsset asset = EditorGUILayout.ObjectField(null, typeof(EnemyAsset), false) as EnemyAsset;
            if (asset)
            {
                (target as Enemy).Use(asset);
            }
        }
    }
#endif
}