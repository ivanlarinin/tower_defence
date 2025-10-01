using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// Base class for spawners that spawn entities (e.g. enemies) at random positions within a defined area.
    /// </summary>
    public abstract class Spawner : MonoBehaviour
    {
        public enum SpawnMode { Start, Loop }
        protected abstract GameObject GenerateSpawnedEntity();
        [SerializeField] private CircleArea m_Area;

        [SerializeField] private EnemyPath m_Path;
        [SerializeField] private EnemyAsset[] m_EnemySettings;
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

        /// <summary>
        /// Spawns the entities and sets their patrol path if they have a TDPatrolController.
        /// </summary>
        private void SpawnEntities()
        {
            for (int i = 0; i < m_NumSpawns; i++)
            {
                // print("Spawning entity");
                var e = GenerateSpawnedEntity();
                e.transform.position = m_Area.GetRandomInsideZone();

                if (e.TryGetComponent<TDPatrolController>(out var patrol))
                    patrol.SetPath(m_Path);

                if (e.TryGetComponent<Enemy>(out var enemy) == true)
                {
                    if (m_EnemySettings != null && m_EnemySettings.Length > 0)
                    {
                        var settings = m_EnemySettings[Random.Range(0, m_EnemySettings.Length)];
                        enemy.Use(settings);
                    }
                }


            }
        }
    }
} 
