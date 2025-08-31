using UnityEngine;

/// <summary>
/// Base class for all projectile types in the game.
/// Handles movement, raycast-based hit detection, and lifetime management.
/// Inherit from this to implement specific projectile behavior (scoring, effects, etc.).
/// </summary>
public abstract class ProjectileBase : Entity
{
    [SerializeField] private float m_Velocity;
    [SerializeField] private float m_Lifetime;
    [SerializeField] private int m_Damage;

    protected virtual void OnHit(Destructable destructible) { }
    protected virtual void OnCollide2D(Collider2D Collider2D) { }
    protected virtual void OnProjectileLifeEnd(Collider2D col, Vector2 pos) { }

    private float m_Timer;
    protected Destructable m_Parent;

    public void SetParentShooter(Destructable parent)
    {
        m_Parent = parent;
    }

    private void Update()
    {
        float steplength = Time.deltaTime * m_Velocity;
        Vector2 step = transform.up * steplength;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, steplength);

        if (hit)
        {
            OnCollide2D(hit.collider);

            Destructable dest = hit.collider.GetComponentInParent<Destructable>();

            if (dest != null)
            {
                Debug.Log($"Projectile hit destructible: {dest.name}");

                dest.ApplyDamage(m_Damage);
                // OnHit(dest);

                OnProjectileLifeEnd(hit.collider, hit.point);
            }
        }

        m_Timer += Time.deltaTime;

        if (m_Timer > m_Lifetime)
            OnProjectileLifeEnd(hit.collider, hit.point);

        transform.position += new Vector3(step.x, step.y, 0);
    }
}