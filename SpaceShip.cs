using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceShip : Destructable
{
    [SerializeField] private Sprite m_PreviewImage; // Sprite used for UI previews of this ship

    [Header("Space Ship")]
    [SerializeField] private float m_Mass;
    [SerializeField] private float m_AngularDampK = 0.1f;
    private Rigidbody2D m_Rigid;

    [Header("Forces")]
    [SerializeField] private float m_MaxThrust = 10f;
    [SerializeField] private float m_MaxTorque = 15f;
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
        m_Rigid.AddForce(transform.up * (ThrustControl * m_MaxThrust), ForceMode2D.Force);

        float torqueCmd = TorqueControl * m_MaxTorque;
        torqueCmd += -m_Rigid.angularVelocity * m_AngularDampK;
        m_Rigid.AddTorque(torqueCmd, ForceMode2D.Force);
    }
    #endregion

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
