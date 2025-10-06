using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace TowerDefence
{
    public class EnemyWave : MonoBehaviour
    {
        [Serializable]
        private struct EnemySquad
        {
            public EnemyAsset asset;
            public int count;
        }

        [Serializable]
        private struct PathGroup
        {
            public EnemySquad[] squads;
        }
        
        [SerializeField] private PathGroup[] groups;
        [SerializeField] private float prepareTime = 10f;

        public float GetRemainTime()
        {
            return Math.Max(0, prepareTime - Time.time);
        }

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            if (Time.time >= prepareTime)
            {
                enabled = false;
                OnWaveReady?.Invoke();
                OnWaveReady = null;
            }
        }

        private event Action OnWaveReady;

        internal void Prepare(Action spawnEnemies)
        {
            prepareTime += Time.time;
            enabled = true;
            OnWaveReady += spawnEnemies;
        }
        public IEnumerable<(EnemyAsset asset, int count, int pathIndex)> EnumerateSquads()
        {
            for (int i = 0; i < groups.Length; i++)
            {
                foreach (var squad in groups[i].squads)
                {
                    yield return (squad.asset, squad.count, i);
                }
            }
        }

        [SerializeField] private EnemyWave nextWave;
        public EnemyWave NextWave => nextWave;
        public EnemyWave PrepareNext(Action spawnEnemies)
        {
            OnWaveReady -= spawnEnemies;
            if (nextWave != null)
            {
                nextWave.Prepare(spawnEnemies);
            }
            return nextWave;
        }
    }
}