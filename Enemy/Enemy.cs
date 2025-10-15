using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// </summary>
namespace TowerDefence
{
    [DisallowMultipleComponent]
    [SelectionBase]
    [RequireComponent(typeof(AIController))]
    [RequireComponent(typeof(CharacterMotor))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private Animator m_animator;
        [SerializeField] private CircleCollider2D m_circleCollider;
        [SerializeField] private Transform m_Scale;
        [SerializeField] private int m_damage = 1;
        [SerializeField] private int m_armor = 1;
        [SerializeField] private int m_gold = 1;
        private AIController m_aiController;
        private CharacterMotor m_characterMotor;

        // Buff related
        [SerializeField] private GameObject m_buffFX;
        [SerializeField] private float m_buffRadius = 2f;
        [SerializeField] private float buffCooldown = 4f;
        private float lastBuffTime = -Mathf.Infinity;
        private int buff = 0;

        [SerializeField] private ArmorType m_ArmorType;

        public event Action OnDeath;

        public enum ArmorType { Base = 0, Mage = 1 }
        private static Func<int, DamageType, int, int>[] ArmorDamageFunctions =
        {
            // Base armor
            (power, type, armor) =>
            {
                switch(type)
                {
                    case DamageType.Magic: return power;
                    default: return Mathf.Min(power - armor, 1);
                }
            },

            // Mage armor
            (power, type, armor) =>
            {
                switch(type)
                {
                    case DamageType.Magic: return Mathf.Min(power - armor, 1);
                    default: return power;
                }
            }
        };


        private Destructable m_destructable;

        private void OnDestroy()
        {
            OnDeath?.Invoke();
        }

        private void Awake()
        {
            CacheRefs();
            if (m_characterMotor != null)
                m_characterMotor.EventOnDeath.AddListener(GiveGold);
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;
            CacheRefs();
        }

        private void Update()
        {
            if (buff == 1)
                TryBuffNearbyEnemy();
        }

        private void TryBuffNearbyEnemy()
        {
            if (Time.time - lastBuffTime < buffCooldown)
                return;

            lastBuffTime = Time.time;

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, m_buffRadius);
            foreach (var hit in hits)
            {
                if (hit.gameObject == this.gameObject) continue;

                CharacterMotor other = hit.GetComponentInParent<CharacterMotor>();
                if (other != null)
                {
                    other.AddHitPoints(10);
                    if (m_buffFX != null)
                    {
                        // Debug.Log($"Buffed {other.name}");
                        Instantiate(m_buffFX, other.transform.position, Quaternion.identity);
                    }
                }
            }
        }

        private void CacheRefs()
        {
            if (!m_spriteRenderer) m_spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
            if (!m_animator) m_animator = GetComponentInChildren<Animator>(true);
            if (!m_circleCollider) m_circleCollider = GetComponentInChildren<CircleCollider2D>(true);

            if (!m_aiController) TryGetComponent(out m_aiController);
            if (!m_characterMotor) TryGetComponent(out m_characterMotor);

            if (!m_Scale && m_spriteRenderer)
                m_Scale = m_spriteRenderer.transform;

            m_destructable = GetComponent<Destructable>();
        }

        public void Use(EnemyAsset asset)
        {
            CacheRefs();

            if (m_spriteRenderer) m_spriteRenderer.color = asset.color;

            if (m_animator && asset.animations)
                m_animator.runtimeAnimatorController = asset.animations;

            if (m_aiController)
                m_aiController.Speed = asset.Speed;

            if (m_characterMotor)
                m_characterMotor.maxSpeed = asset.Speed;

            if (m_circleCollider)
                m_circleCollider.radius = asset.Radius;

            if (m_Scale)
            {
                float scale = asset.Radius * 4f;
                m_Scale.localScale = new Vector3(scale, scale, 1f);
            }

            m_damage = asset.damage;
            m_gold = asset.gold;
            buff = asset.buff;
            m_armor = asset.armor;
        }

        public void DamagePlayer()
        {
            // TODO: Damage the player with m_damage
        }

        public void GiveGold()
        {
            TDPlayer player = FindFirstObjectByType<TDPlayer>();
            if (player != null)
            {
                player.ChangeGold(m_gold);
            }
        }

        public void TakeDamage(int damage, DamageType damageType)
        {
            if (m_destructable != null)
            {
                int finalDamage = ArmorDamageFunctions[(int)m_ArmorType](damage, damageType, m_armor);
                m_destructable.ApplyDamage(finalDamage, damageType);
            }
        }

    }

    [CustomEditor(typeof(Enemy))]
    public class EnemyInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EnemyAsset a = EditorGUILayout.ObjectField(null, typeof(EnemyAsset), false) as EnemyAsset;
            if (a)
            {
                (target as Enemy).Use(a);
            }
        }
    }
}
