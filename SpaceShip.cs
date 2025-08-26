using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceShip : Destructable
{
    [SerializeField] private Sprite m_PreviewImage; // Sprite used for UI previews of this ship

    [Header("Space Ship")]
    [SerializeField] private float m_Mass;               // Rigidbody mass
    [SerializeField] private float m_Thrust;             // Forward thrust force
    [SerializeField] private float m_Mobility;           // Turning/maneuverability factor
    [SerializeField] private float m_MaxLinearVelocity;  // Maximum forward/backward speed
    [SerializeField] private float m_MaxAngularVelocity; // Maximum rotational speed (degrees/sec)

    private Rigidbody2D m_Rigid; // Cached Rigidbody2D reference

    public float MaxLinearVelocity => m_MaxLinearVelocity;
    public float MaxAngularVelocity => m_MaxAngularVelocity;
    public Sprite PreviewImage => m_PreviewImage;

    #region Public API

    /// <summary>
    /// Forward thrust control input, from -1.0 (reverse) to +1.0 (forward).
    /// </summary>
    public float ThrustControl { get; set; }

    /// <summary>
    /// Rotation control input, from -1.0 (turn left) to +1.0 (turn right).
    /// </summary>
    public float TorqueControl { get; set; }

    #endregion

    #region Unity Events
    protected override void Start()
    {
        base.Start();

        // Initialize rigidbody with defined mass and inertia
        m_Rigid = GetComponent<Rigidbody2D>();
        m_Rigid.mass = m_Mass;
        m_Rigid.inertia = 1;

        // Initialize energy/ammo
        // InitOffensive();
    }

    private void Update()
    {
        // Currently unused — placeholder for frame-based logic
    }

    private void FixedUpdate()
    {
        UpdateRigidBody();    // Apply thrust/rotation forces
        // UpdateEnergyRegen();  // Regenerate primary energy over time
    }
    #endregion

    /// <summary>
    /// Applies movement and rotation forces based on player/AI control values.
    /// </summary>
    private void UpdateRigidBody()
    {
        // Forward/backward thrust
        m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

        // Linear drag proportional to current velocity and mobility
        m_Rigid.AddForce(-m_Rigid.linearVelocity * (m_Mobility / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

        // Rotational thrust
        m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

        // Rotational drag proportional to current angular velocity and mobility
        m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    /// <summary>
    /// TODO: Change the method to the actual one
    /// Used by Turrets
    /// </summary>
    public bool DrawEnergy(int count)
    {
        return false;
    }

    /// <summary>
    /// TODO: Write a method
    /// Used by Turrets
    /// </summary>
    public bool DrawAmmo(int count)
    {
        return false;
    }

    /// <summary>
    /// Fires all turrets of the specified mode (Primary/Secondary).
    /// </summary>
    public void Fire(TurretMode mode)
    {
        return;
    }
    
/*
                            #region Offensive

                            [SerializeField] private Turret[] m_Turrets; // Turrets mounted on this ship

                            [Header("Offensive Stats")]
                            [SerializeField] private int m_MaxEnergy;              // Maximum energy for primary weapons
                            [SerializeField] private int m_MaxAmmo;                // Maximum ammo for secondary weapons
                            [SerializeField] private int m_EnergyRegenPerSecond;   // Energy regenerated per second

                            private float m_PrimaryEnergy; // Current primary weapon energy
                            private int m_SecondaryAmmo;   // Current secondary weapon ammo

                            /// <summary>
                            /// Adds energy to the primary weapon pool, clamped to max.
                            /// </summary>
                            public void AddEnergy(int e)
                            {
                                m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
                            }

                            /// <summary>
                            /// Adds ammo to the secondary weapon pool, clamped to max.
                            /// </summary>
                            public void AddAmmo(int ammo)
                            {
                                m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
                            }

                            /// <summary>
                            /// Initializes energy and ammo to max values.
                            /// </summary>
                            private void InitOffensive()
                            {
                                m_PrimaryEnergy = m_MaxEnergy;
                                m_SecondaryAmmo = m_MaxAmmo;
                            }

                            /// <summary>
                            /// Regenerates primary weapon energy each fixed update.
                            /// </summary>
                            private void UpdateEnergyRegen()
                            {
                                m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
                                m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
                            }

                            /// <summary>
                            /// Attempts to consume energy for firing a primary weapon.
                            /// </summary>
                            public bool DrawEnergy(int count)
                            {
                                if (count == 0) return true;

                                if (m_PrimaryEnergy >= count)
                                {
                                    m_PrimaryEnergy -= count;
                                    return true;
                                }
                                return false;
                            }

                            /// <summary>
                            /// Attempts to consume ammo for firing a secondary weapon.
                            /// </summary>
                            public bool DrawAmmo(int count)
                            {
                                if (count == 0) return true;

                                if (m_SecondaryAmmo >= count)
                                {
                                    m_SecondaryAmmo -= count;
                                    return true;
                                }
                                return false;
                            }

                            /// <summary>
                            /// Assigns a new weapon loadout to all turrets on this ship.
                            /// </summary>
                            public void AssignWeapon(TurretProperties props)
                            {
                                for (int i = 0; i < m_Turrets.Length; i++)
                                {
                                    m_Turrets[i].AssignLoadout(props);
                                }
                            }

                            #endregion
                        */
}
