using UnityEditor;
using UnityEngine;

/// <summary>
/// Enemy component. Requires an AIController and CharacterMotor2D component.
/// </summary>
namespace TowerDefence
{
    [DisallowMultipleComponent]
    [SelectionBase]
    [RequireComponent(typeof(AIController))]
    [RequireComponent(typeof(CharacterMotor2D))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private Animator m_animator;
        [SerializeField] private CircleCollider2D m_circleCollider;
        [SerializeField] private Transform m_Scale;
        [SerializeField] private int m_damage = 1;


        private AIController m_aiController;
        private CharacterMotor2D m_characterMotor;

        private void Awake()
        {
            CacheRefs();
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;
            CacheRefs();
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
                float scale = asset.Radius * 2f;
                m_Scale.localScale = new Vector3(scale, scale, 1f);
            }

            m_damage = asset.damage;
        }

        public void DamagePlayer()
        {
            Player.Instance.TakeDamage(m_damage);       }
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
