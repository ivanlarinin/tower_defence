using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


namespace TowerDefence
{
    public class Destructable : Entity
    {
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

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        /// <summary>
        /// Set the team ID for this destructible object.
        /// </summary>
        /// <param name="teamId">Team identifier to assign.</param>
        public void SetTeamId(int teamId)
        {
            m_TeamId = teamId;
        }

        public void AddHitPoints(int amount)
        {
            m_CurrentHitPoints += amount;
            if (m_CurrentHitPoints > m_HitPoints)
                m_CurrentHitPoints = m_HitPoints;
        }

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        /// <summary>
        /// Called when hit points drop to zero or below. Triggers VFX and destruction.
        /// </summary>
        protected virtual void OnDeath()
        {
            // Spawn explosion VFX at the current transform position
            // if (m_ExplosionPrefab != null)
            // {
            //     Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);
            // }

            // Destroy this object and fire the death event
            Destroy(gameObject);
            m_EventOnDeath?.Invoke();
        }

        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;
    }
}