using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace TowerDefence
{
    public class EnemyWavesManager1 : MonoBehaviour
    {
        [SerializeField] private Enemy m_EnemyPrefab;
        [SerializeField] private EnemyPath[] paths;
        [SerializeField] private EnemyAsset[] enemyAssets;

        private Dictionary<string, EnemyAsset> assetMap;
        private WaveConfig waveConfig;
        private int currentWaveIndex = 0;

        void Start()
        {
            assetMap = new Dictionary<string, EnemyAsset>();
            foreach (var ea in enemyAssets)
                assetMap[ea.name] = ea;

            LoadWaveJson();
            StartWave(currentWaveIndex);
        }

        private void LoadWaveJson()
        {
            string path = Path.Combine(Application.dataPath, "waves.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                waveConfig = JsonUtility.FromJson<WaveConfig>(json);
                Debug.Log("Loaded waves.json");
            }
            else
            {
                Debug.LogError("waves.json not found!");
                waveConfig = new WaveConfig();
            }
        }

        private void StartWave(int waveIndex)
        {
            if (waveConfig.waves.Count <= waveIndex) return;

            WaveData wave = waveConfig.waves[waveIndex];
            Invoke(nameof(SpawnWave), wave.prepareTime);
        }

        private void SpawnWave()
        {
            if (waveConfig.waves.Count <= currentWaveIndex) return;

            WaveData wave = waveConfig.waves[currentWaveIndex];

            for (int g = 0; g < wave.groups.Count; g++)
            {
                GroupData group = wave.groups[g];
                if (group.pathIndex >= paths.Length) continue;

                for (int s = 0; s < group.squads.Count; s++)
                {
                    SquadData squad = group.squads[s];
                    if (!assetMap.ContainsKey(squad.asset)) continue;

                    for (int i = 0; i < squad.count; i++)
                    {
                        var e = Instantiate(m_EnemyPrefab, paths[group.pathIndex].SpawnArea.GetRandomInsideZone(), Quaternion.identity);
                        e.Use(assetMap[squad.asset]);
                        e.GetComponent<TDPatrolController>().SetPath(paths[group.pathIndex]);
                    }
                }
            }

            currentWaveIndex++;
            if (currentWaveIndex < waveConfig.waves.Count)
            {
                StartWave(currentWaveIndex);
            }
        }
    }
}
