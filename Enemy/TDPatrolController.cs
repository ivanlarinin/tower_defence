using UnityEngine;
using UnityEngine.Events;

namespace TowerDefence
{
    [RequireComponent(typeof(AIController))]
    public class TDPatrolController : MonoBehaviour
    {
        [Header("Patrol Route")]
        [SerializeField] private Path m_Path;
        public void SetPath(Path path) => m_Path = path;

        private AIController m_AI;
        [SerializeField] private UnityEvent m_OnArrive;
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
                m_OnArrive?.Invoke();
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
            {
                m_OnArrive?.Invoke();

                if (TDPlayer.Instance != null)
                {
                    TDPlayer.Instance.ReduceLife(1);
                }
                Destroy(gameObject);
                return;
            }

            m_AI.SetMoveTarget(m_Path.GetPosition(m_Index));
        }
    }
}
