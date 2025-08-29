using UnityEngine;

[DisallowMultipleComponent] // Prevent multiple Turret scripts from being added to the same GameObject
public class Turret : MonoBehaviour
{
    [SerializeField] private TurretMode m_Mode;                  // Whether this turret is Primary or Secondary
    public TurretMode Mode => m_Mode;                            // Public read-only access to turret mode

    [SerializeField] private TurretProperties m_TurretProperties; // ScriptableObject defining projectile, fire rate, ammo/energy usage

    private float m_RefireTimer;                                 // Time remaining until turret can fire again

    public bool CanFire => m_RefireTimer <= 0;                   // True if turret can shoot right now

    private SpaceShip m_Ship;                                    // The ship this turret is attached to

    private void Start()
    {
        // Cache reference to parent ship (used for ammo/energy checks)
        m_Ship = GetComponentInParent<SpaceShip>();
    }

    private void Update()
    {
        // Countdown refire timer each frame
        if (m_RefireTimer > 0)
        {
            m_RefireTimer -= Time.deltaTime;
        }
        else if (Mode == TurretMode.Auto)
        {
            Fire(); // Auto turrets try to fire as soon as they can
        }
    }

    /// <summary>
    /// Attempts to fire the turret. Will fail if conditions aren't met (no ammo/energy, still on cooldown, etc.).
    /// </summary>
    public void Fire()
    {
        if (m_TurretProperties == null) return;  // No weapon assigned
        if (m_Ship == null) return;              // No parent ship found
        if (m_RefireTimer > 0) return;           // Still cooling down

        // Primary turrets consume energy, secondary turrets consume ammo
        if (m_Mode == TurretMode.Primary)
        {
            if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false)
                return; // Not enough energy
        }
        else if (m_Mode == TurretMode.Secondary)
        {
            if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false)
                return; // Not enough ammo
        }

        // Spawn projectile from the turret's properties
        Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
        Debug.Log($"Spawned projectile");

        // Position and orient projectile to match turret
        projectile.transform.position = transform.position;
        projectile.transform.up = transform.up;

        // Record the shooter so the projectile won't damage its own ship
        projectile.SetParentShooter(m_Ship);

        // Reset refire cooldown
        m_RefireTimer = m_TurretProperties.RateOfFire;

        {
            // TODO: Add sound effects / muzzle flash here
        }
    }

    /// <summary>
    /// Assigns a new weapon to this turret, but only if its mode matches.
    /// </summary>
    /// <param name="props">TurretProperties for the new weapon.</param>
    public void AssignLoadout(TurretProperties props)
    {
        if (m_Mode != props.Mode) return; // Ignore if weapon mode doesn't match turret mode

        m_RefireTimer = 0;                // Reset cooldown so it can fire immediately
        m_TurretProperties = props;       // Assign new weapon properties
    }
}
