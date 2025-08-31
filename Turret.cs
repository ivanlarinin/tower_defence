using UnityEngine;

[DisallowMultipleComponent]
public class Turret : MonoBehaviour
{
    [SerializeField] private TurretMode m_Mode;
    public TurretMode Mode => m_Mode;

    [SerializeField] private TurretProperties m_TurretProperties;

    private float m_RefireTimer;

    public bool CanFire => m_RefireTimer <= 0;

    private void Start()
    {
    }

    private void Update()
    {
        if (m_RefireTimer > 0)
        {
            m_RefireTimer -= Time.deltaTime;
        }
        else if (Mode == TurretMode.Auto)
        {
            // Fire();
        }
    }

    public void Fire()
    {
        if (m_TurretProperties == null) return;
        if (m_RefireTimer > 0) return;

        Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.transform.up = transform.up;

        m_RefireTimer = m_TurretProperties.RateOfFire;

        {
            // TODO: Add sound effects / muzzle flash here
        }
    }
}
