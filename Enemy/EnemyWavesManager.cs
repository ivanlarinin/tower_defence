using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class EnemyWavesManager : MonoBehaviour
    {
        public static event Action<Enemy> OnEnemySpawn;
        [SerializeField] private Enemy m_EnemyPrefab;
        [SerializeField] private EnemyPath[] paths;
        [SerializeField] private EnemyAsset[] enemyAssets;
        public event Action OnAllWavesCompleted;

        private Dictionary<string, EnemyAsset> assetMap;
        private WaveConfig waveConfig;
        private int currentWaveIndex = 0;
        private int activeEnemyCount = 0;

        public Action<int> OnWaveNumberChanged { get; private set; }

        private void RecordEnemyDeath()
        {
            activeEnemyCount--;
            if (activeEnemyCount <= 0 && currentWaveIndex >= waveConfig.waves.Count)
            {
                OnAllWavesCompleted?.Invoke();
            }
        }

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
            string sceneName = SceneManager.GetActiveScene().name;
            TextAsset jsonFile = Resources.Load<TextAsset>($"{sceneName}_waves");
            if (jsonFile != null)
            {
                waveConfig = JsonUtility.FromJson<WaveConfig>(jsonFile.text);
                Debug.Log("Loaded waves.json from Resources");
            }
            else
            {
                Debug.LogError("waves.json not found in Resources!");
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
            OnWaveNumberChanged?.Invoke(currentWaveIndex + 1);

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
                        e.DeathEvent += RecordEnemyDeath;
                        e.Use(assetMap[squad.asset]);
                        e.GetComponent<TDPatrolController>().SetPath(paths[group.pathIndex]);
                        activeEnemyCount++;
                        OnEnemySpawn?.Invoke(e);
                    }
                }
            }

            currentWaveIndex++;
            if (currentWaveIndex < waveConfig.waves.Count)
            {
                StartWave(currentWaveIndex);
            }
        }

        public bool IsLastWaveCompleted()
        {
            return currentWaveIndex >= waveConfig.waves.Count;
        }

        public void CallNextWave()
        {
            if (currentWaveIndex < waveConfig.waves.Count)
            {
                CancelInvoke(nameof(SpawnWave));
                SpawnWave();
            }
        }

        internal static void WaveNumberUpdateSubscribe(Action<int> updateText)
        {
            EnemyWavesManager instance = FindFirstObjectByType<EnemyWavesManager>();
            if (instance != null)
            {
                instance.OnWaveNumberChanged += updateText;
                updateText(instance.currentWaveIndex);
            }
        }

        internal static void WaveNumberUpdateUnsubscribe(Action<int> updateText)
        {
            EnemyWavesManager instance = FindFirstObjectByType<EnemyWavesManager>();
            if (instance != null)
            {
                instance.OnWaveNumberChanged -= updateText;
            }
        }
    }
}
