using UnityEngine;

namespace TowerDefence
{
    public class EntitySpawner : MonoBehaviour
    {
        public enum SpawnMode { Start, Loop }

        [SerializeField] private Path m_Path;
        [SerializeField] private Entity[] m_EntityPrefabs;
        [SerializeField] private CircleArea m_Area;
        [SerializeField] private SpawnMode m_SpawnMode = SpawnMode.Start;
        [SerializeField] private int m_NumSpawns = 1;
        [SerializeField] private float m_RespawnTime = 5f;

        private float m_Timer;

        private void Start()
        {
            if (m_SpawnMode == SpawnMode.Start) SpawnEntities();
            m_Timer = m_RespawnTime;
        }

        private void Update()
        {
            if (m_SpawnMode != SpawnMode.Loop) return;

            m_Timer -= Time.deltaTime;
            if (m_Timer <= 0f)
            {
                SpawnEntities();
                m_Timer = m_RespawnTime;
            }
        }

        private void SpawnEntities()
        {
            for (int i = 0; i < m_NumSpawns; i++)
            {
                int index = Random.Range(0, m_EntityPrefabs.Length);
                GameObject go = Instantiate(m_EntityPrefabs[index].gameObject);
                go.transform.position = m_Area != null ? m_Area.GetRandomInsideZone() : transform.position;

                // Ensure the spawned unit has a TDPatrolController with the path assigned.
                if (go.TryGetComponent<TDPatrolController>(out var patrol))
                {
                    // If the prefab wasn’t prewired, wire it here:
                    var field = typeof(TDPatrolController).GetField("m_Path", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (field != null && field.GetValue(patrol) == null)
                    {
                        field.SetValue(patrol, m_Path);
                    }
                }
            }
        }
    }
}
