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

        /// <summary>
        /// Called when hit points reach zero, triggering death logic.
        /// </summary>
        protected virtual void OnDeath()
        {
            DeathEvent?.Invoke();
            Destroy(gameObject);
        }


        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;
    }
}