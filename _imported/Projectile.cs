using UnityEngine;

/// <summary>
/// A specific projectile type that awards score and kills to the player
/// when it destroys enemies, and spawns an impact effect on hit.
/// Inherits from ProjectileBase for movement and hit detection.
/// </summary>
public class Projectile : ProjectileBase
{
    [SerializeField] private ImpactEffect m_ImpactEffectPrefab;

    protected override void OnHit(Destructable destructible)
    {
        if (m_Parent == Player.Instance.ActiveShip)
        {
            Player.Instance.AddScore(destructible.ScoreValue);

            if (destructible is SpaceShip)
            {
                if (destructible.HitPoints <= 0)
                {
                    Player.Instance.AddKill();
                }
            }
        }
    }

    protected override void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
    {
        if (m_ImpactEffectPrefab != null)
        {
            Instantiate(m_ImpactEffectPrefab, pos, Quaternion.identity);
        }

        Destroy(gameObject, 0);
    }
}