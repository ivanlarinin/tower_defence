using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// A destructible entity in the scene—anything that can have hit points and be destroyed.
/// </summary>
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

    /// <summary>
    /// Apply damage to this object.
    /// </summary>
    /// <param name="damage">Amount of damage to apply.</param>
    public void ApplyDamage(int damage)
    {
        if (m_Indestructible) return;

        // Check for a shield system first and let it absorb what it can
        // ShieldSystem shieldSystem = GetComponent<ShieldSystem>();
        // if (shieldSystem != null && shieldSystem.HasShield)
        // {
        //     //Debug.Log($"Projectile dmg={damage}");
        //     //Debug.Log($"Shield ={shieldSystem.ShieldHealth}");

        //     float remainingDamage = shieldSystem.AbsorbDamage(damage);
        //     damage = Mathf.RoundToInt(remainingDamage);

        //     // If the shield absorbed everything, don't apply hull damage
        //     if (damage <= 0) return;
        // }

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

    [SerializeField] private UnityEvent m_EventOnDeath;
    public UnityEvent EventOnDeath => m_EventOnDeath;

    /// <summary>
    /// Called when hit points drop to zero or below. Triggers VFX and destruction.
    /// </summary>
    protected virtual void OnDeath()
    {
        // Spawn explosion VFX at the current transform position
        if (m_ExplosionPrefab != null)
        {
            Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);
        }

        // Destroy this object and fire the death event
        Destroy(gameObject);
        m_EventOnDeath?.Invoke();
    }

    [SerializeField] private int m_ScoreValue;
    public int ScoreValue => m_ScoreValue;
}
