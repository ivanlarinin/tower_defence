using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// Controls level flow, including win/loss conditions and score calculation.
    /// </summary>
    public class LevelController : SingletonBase<LevelController>
    {

        [Header("Score Settings")]
        [SerializeField] private float maxTimeForFullScore = 40f;
        [SerializeField] private int maxGoldUsedForFullScore = 10;

        private bool m_IsLevelCompleted;
        private float m_LevelTime;

        [SerializeField] private GameObject hudPrefab;
        private LevelResult resultPanel;

        private void Start()
        {
            TDPlayer.Instance.PlayerDied += Lose;

            var hudGO = Instantiate(hudPrefab);
            resultPanel = hudGO.GetComponentInChildren<LevelResult>();

            var wavesManager = FindFirstObjectByType<EnemyWavesManager>();
            if (wavesManager != null)
                wavesManager.OnAllWavesCompleted += OnLevelCompleted;
        }

        private void Update()
        {
            if (m_IsLevelCompleted)
                return;

            m_LevelTime += Time.deltaTime;
        }

        private void OnLevelCompleted()
        {
            if (m_IsLevelCompleted)
                return;

            m_IsLevelCompleted = true;
            StopLevelActivity();
            resultPanel.ShowResult(true);

            var currentEpisode = LevelSequenceController.Instance.CurrentEpisode;
            MapCompletion.SaveEpisodeResult(currentEpisode, LevelScore);

        }

        private void Lose()
        {
            StopLevelActivity();
            resultPanel.ShowResult(false);
        }

        private void StopLevelActivity()
        {
            void DisableAll<T>() where T : MonoBehaviour
            {
                foreach (var comp in FindObjectsByType<T>(FindObjectsSortMode.None))
                    comp.enabled = false;
            }

            DisableAll<Enemy>();
            DisableAll<Enemy>();
            DisableAll<Spawner>();
            DisableAll<Projectile>();
            DisableAll<Tower>();
            DisableAll<CallNextWave>();
        }

        public void OnPlayNext()
        {
            LevelSequenceController.Instance.AdvanceLevel();
            resultPanel.HideAll();
        }
        public void OnRestartLevel() => LevelSequenceController.Instance.RestartLevel();
        public void OnExitToMenu() => LevelSequenceController.Instance.ExitToMenu();

        /// <summary>
        /// Calculates the number of stars (1-3) based on performance.
        /// </summary>
        public int LevelScore
        {
            get
            {
                int stars = 3;

                // 1. Time penalty
                if (m_LevelTime > maxTimeForFullScore)
                    stars--;

                // 2. Lives lost penalty
                if (TDPlayer.Instance.NumLives > 3)
                    stars--;

                // 3. Gold usage penalty
                if (TDPlayer.Instance.GoldSpent > maxGoldUsedForFullScore)
                    stars--;

                return Mathf.Clamp(stars, 1, 3);
            }
        }
    }
}
