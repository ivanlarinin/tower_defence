using UnityEngine;

namespace TowerDefence
{
    [RequireComponent(typeof(AIController))]
    public class TDPatrolController : MonoBehaviour
    {
        [Header("Patrol Route")]
        [SerializeField] private Path m_Path;

        private AIController m_AI;
        private int m_Index = -1;

        private void Awake()
        {
            m_AI = GetComponent<AIController>();
        }

        private void Start()
        {
            if (m_Path == null || m_Path.Count == 0)
            {
                Debug.LogWarning($"[{name}] No path/points, destroying.");
                Destroy(gameObject);
                return;
            }

            // Start at first point
            AdvanceToNextPoint();
        }

        private void Update()
        {
            // When we arrive, go to next point or destroy
            if (m_AI.ReachedTarget())
            {
                AdvanceToNextPoint();
            }
        }

        private void AdvanceToNextPoint()
        {
            m_Index++;

            if (m_Path == null || m_Index >= m_Path.Count)
                Destroy(gameObject);

            m_AI.SetMoveTarget(m_Path.GetPosition(m_Index));
        }
    }
}
