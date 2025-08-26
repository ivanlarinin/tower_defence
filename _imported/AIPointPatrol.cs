using UnityEngine;
using System.Collections.Generic;

public class AIPointPatrol : MonoBehaviour
{
    [SerializeField] private float m_Radius;
    public float Radius => m_Radius;
    
    /// <summary>
    /// Sets the patrol radius
    /// </summary>
    /// <param name="radius">The new radius value</param>
    public void SetRadius(float radius)
    {
        m_Radius = radius;
    }

    [Header("Patrol Route")]
    [SerializeField] private List<Transform> m_PatrolPoints = new List<Transform>();
    [SerializeField] private bool m_UsePatrolRoute = false;
    [SerializeField] private float m_PatrolPointReachDistance = 2.0f;
    [SerializeField] private bool m_LoopPatrolRoute = true;

    private int m_CurrentPatrolIndex = 0;

    public bool UsePatrolRoute => m_UsePatrolRoute;
    public int CurrentPatrolIndex => m_CurrentPatrolIndex;
    public int PatrolPointsCount => m_PatrolPoints.Count;

    private static readonly Color GizmoColor = new Color(1, 0, 0, 0.3f);
    private static readonly Color RouteColor = new Color(0, 1, 0, 0.8f);

    public Vector3 GetCurrentPatrolPoint()
    {
        if (!m_UsePatrolRoute || m_PatrolPoints.Count == 0)
            return transform.position;

        if (m_CurrentPatrolIndex >= m_PatrolPoints.Count)
            return transform.position;

        return m_PatrolPoints[m_CurrentPatrolIndex].position;
    }

    public Vector3 GetNextPatrolPoint()
    {
        if (!m_UsePatrolRoute || m_PatrolPoints.Count == 0)
            return transform.position;

        int nextIndex = m_CurrentPatrolIndex + 1;
        
        if (m_LoopPatrolRoute)
        {
            nextIndex = nextIndex % m_PatrolPoints.Count;
        }
        else
        {
            nextIndex = Mathf.Min(nextIndex, m_PatrolPoints.Count - 1);
        }

        return m_PatrolPoints[nextIndex].position;
    }

    public bool IsAtPatrolPoint(Vector3 position)
    {
        if (!m_UsePatrolRoute || m_PatrolPoints.Count == 0)
            return false;

        Vector3 currentPoint = GetCurrentPatrolPoint();
        float distance = Vector3.Distance(position, currentPoint);
        return distance <= m_PatrolPointReachDistance;
    }

    public void AdvanceToNextPatrolPoint()
    {
        if (!m_UsePatrolRoute || m_PatrolPoints.Count == 0)
            return;

        m_CurrentPatrolIndex++;

        if (m_LoopPatrolRoute)
        {
            m_CurrentPatrolIndex = m_CurrentPatrolIndex % m_PatrolPoints.Count;
        }
        else
        {
            m_CurrentPatrolIndex = Mathf.Min(m_CurrentPatrolIndex, m_PatrolPoints.Count - 1);
        }
    }

    public void ResetPatrolRoute()
    {
        m_CurrentPatrolIndex = 0;
    }

    public void SetPatrolPoints(List<Transform> points, bool loopRoute = true)
    {
        m_PatrolPoints.Clear();
        m_PatrolPoints.AddRange(points);
        m_LoopPatrolRoute = loopRoute;
        m_UsePatrolRoute = points.Count > 0;
        m_CurrentPatrolIndex = 0;
    }

    public void AddPatrolPoint(Transform point)
    {
        if (point != null)
        {
            m_PatrolPoints.Add(point);
            m_UsePatrolRoute = true;
        }
    }

    public void ClearPatrolPoints()
    {
        m_PatrolPoints.Clear();
        m_UsePatrolRoute = false;
        m_CurrentPatrolIndex = 0;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw current patrol zone
        Gizmos.color = GizmoColor;
        Gizmos.DrawSphere(transform.position, m_Radius);

        // Draw patrol route if enabled
        if (m_UsePatrolRoute && m_PatrolPoints.Count > 0)
        {
            Gizmos.color = RouteColor;
            
            for (int i = 0; i < m_PatrolPoints.Count; i++)
            {
                if (m_PatrolPoints[i] != null)
                {
                    // Draw patrol points
                    Gizmos.DrawWireSphere(m_PatrolPoints[i].position, 0.5f);
                    
                    // Draw lines between points
                    if (i < m_PatrolPoints.Count - 1 && m_PatrolPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(m_PatrolPoints[i].position, m_PatrolPoints[i + 1].position);
                    }
                    
                    // Draw line from last to first if looping
                    if (m_LoopPatrolRoute && i == m_PatrolPoints.Count - 1 && m_PatrolPoints[0] != null)
                    {
                        Gizmos.DrawLine(m_PatrolPoints[i].position, m_PatrolPoints[0].position);
                    }
                }
            }

            // Highlight current patrol point
            if (m_CurrentPatrolIndex < m_PatrolPoints.Count && m_PatrolPoints[m_CurrentPatrolIndex] != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(m_PatrolPoints[m_CurrentPatrolIndex].position, 1.0f);
            }
        }
    }
}
