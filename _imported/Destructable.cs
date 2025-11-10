using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;


namespace TowerDefence
{
    public class Destructable : Entity
    {
        public event Action DeathEvent;

        #region Properties

        /// <summary>
        /// If true, this object ignores all incoming damage.
        /// </summary>
        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Starting/max hit points.
        /// </summary>
        [SerializeField] private int m_HitPoints;
        public int MaxHitPoints => m_HitPoints;

        /// <summary>
        /// Current hit points.
        /// </summary>
        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        [Header("Explosion")]
        [SerializeField] private GameObject m_ExplosionPrefab;

        [Header("Sound")]
        [SerializeField] private Sound m_DestructionSound;

        #endregion

        #region Unity Events

        protected virtual void Start()
        {
            // Initialize current HP from starting value
            m_CurrentHitPoints = m_HitPoints;
        }

        #endregion

        #region Public API

        public void ApplyDamage(int damage, DamageType damageType)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0) 
                OnDeath();
        }

        #endregion

        private static HashSet<Destructable> m_AllDestructibles;
        public static IReadOnlyCollection<Destructable> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
            {
                m_AllDestructibles = new HashSet<Destructable>();
            }
            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }

        public void AddHitPoints(int amount)
        {
            m_CurrentHitPoints += amount;
            if (m_CurrentHitPoints > m_HitPoints)
                m_CurrentHitPoints = m_HitPoints;
        }

        [SerializeField] private SpriteRenderer m_SpriteRenderer;

        protected virtual SpriteRenderer GetSpriteRenderer()
        {
            return GetComponentInChildren<SpriteRenderer>();
        }

        [SerializeField] private float m_BlinkDuration = 0.5f;
        [SerializeField] private float m_BlinkInterval = 0.1f;
        [SerializeField] private bool m_BlinkOnDeath = true;


        /// <summary>
        /// Called when hit points reach zero, triggering death logic.
        /// </summary>
        protected virtual void OnDeath()
        {
            m_DestructionSound.Play();
            DeathEvent?.Invoke();

            var renderer = GetSpriteRenderer();
            if (renderer != null)
                StartCoroutine(BlinkAndDestroy(renderer));
            else
                Destroy(gameObject);
        }

        private System.Collections.IEnumerator BlinkAndDestroy(SpriteRenderer renderer)
        {
            float elapsed = 0f;
            bool visible = true;

            while (elapsed < m_BlinkDuration)
            {
                visible = !visible;
                m_SpriteRenderer.enabled = visible;
                yield return new WaitForSeconds(m_BlinkInterval);
                elapsed += m_BlinkInterval;
            }

            m_SpriteRenderer.enabled = true;
            Destroy(gameObject);
        }



        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;
    }
}