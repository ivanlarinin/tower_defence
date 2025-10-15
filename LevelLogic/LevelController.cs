using UnityEngine;

namespace TowerDefence
{
    public class LevelController : SingletonBase<LevelController>
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject m_PanelSuccess;
        [SerializeField] private GameObject m_PanelFailure;

        [Header("Score Settings")]
        [SerializeField] private float maxTimeForFullScore = 40f;
        [SerializeField] private int maxGoldUsedForFullScore = 10;

        private bool m_IsLevelCompleted;
        private float m_LevelTime;

        public float LevelTime => m_LevelTime;

        private void Start()
        {
            TDPlayer.Instance.PlayerDied += Lose;
        
            var wavesManager = FindObjectsByType<EnemyWavesManager>(FindObjectsSortMode.None)[0];
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

            var currentEpisode = LevelSequenceController.Instance.CurrentEpisode;
            MapCompletion.SaveEpisodeResult(currentEpisode, LevelScore);

            Show(true);
        }

        private void Lose()
        {
            StopLevelActivity();
            Show(false);
        }

        private void StopLevelActivity()
        {
            void DisableAll<T>() where T : MonoBehaviour
            {
                foreach (var comp in FindObjectsByType<T>(FindObjectsSortMode.None))
                    comp.enabled = false;
            }

            DisableAll<CharacterMotor>();
            DisableAll<Enemy>();
            DisableAll<Spawner>();
            DisableAll<Projectile>();
            DisableAll<Tower>();
            DisableAll<CallNextWave>();
        }

        private void Show(bool success)
        {
            m_PanelSuccess?.SetActive(success);
            m_PanelFailure?.SetActive(!success);
        }

        public void OnPlayNext()
        {
            LevelSequenceController.Instance.AdvanceLevel();
            m_PanelSuccess?.SetActive(false);
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
