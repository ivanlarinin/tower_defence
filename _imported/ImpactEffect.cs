using UnityEngine;

/// <summary>
/// Simple visual effect script that destroys the GameObject after a set lifetime.
/// Commonly used for explosion flashes, hit sparks, etc.
/// </summary>
public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private float m_lifetime;
    private float m_Timer;

    private void Update()
    {
        if (m_Timer < m_lifetime)
            m_Timer += Time.deltaTime;
        else
            Destroy(gameObject);
    }
}