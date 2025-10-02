using System.IO;
using UnityEngine;

namespace TowerDefence
{
    public class EnemyWavesManager : MonoBehaviour
    {
        [SerializeField] private Enemy m_EnemyPrefab; 
        [SerializeField] private EnemyPath[] paths;
        [SerializeField] private EnemyWave currentWave;

        void Start()
        {
            if (currentWave == null) return;
            currentWave.Prepare(SpawnEnemies);
        }

        private void SpawnEnemies()
        {
            foreach ((EnemyAsset asset, int count, int pathIndex) in currentWave.EnumerateSquads())
            {
                if (pathIndex < paths.Length)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var e = Instantiate(m_EnemyPrefab);
                        e.Use(asset);
                        e.GetComponent<TDPatrolController>().SetPath(paths[pathIndex]);
                    }
                }
                else
                {
                    Debug.LogError($"Path index {pathIndex} is out of range. Available paths: {paths.Length}");
                }
            }
            currentWave = currentWave.PrepareNext(SpawnEnemies);
        }
    }
}