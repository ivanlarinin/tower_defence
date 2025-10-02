using UnityEngine;
using UnityEngine.Events;

namespace TowerDefence
{
    public interface ILevelCondition
    {
        bool IsCompleted { get; }
    }
    public class LevelController : SingletonBase<LevelController>
    {
        [SerializeField] private LevelCondition[] m_Conditions;

        [SerializeField] private GameObject m_PanelSuccess;
        [SerializeField] private GameObject m_PanelFailure;

        private bool m_IsLevelCompleted;
        private float m_LevelTime;
        public int levelScore => 1;

        // public bool HasNextLevel => m_LevelProperties.NextLevel != null;
        public float LevelTime => m_LevelTime;

        private int m_TimeLifePenalty = 0;

        #region Unity events

        private void Start()
        {
            m_Conditions = GetComponentsInChildren<LevelCondition>();
            TDPlayer.Instance.PlayerDied += Lose;
        }

        private void Update()
        {
            if (m_IsLevelCompleted == false)
            {
                m_LevelTime += Time.deltaTime;
                CheckLevelConditions();

                if (m_Conditions[0].IsCompleted == true && m_TimeLifePenalty == 0)
                {
                    TDPlayer.Instance.ReduceLife(1);
                    m_TimeLifePenalty = 1;
                }
            }
        }

        #endregion

        public void Show(bool result)
        {
            m_PanelSuccess?.gameObject.SetActive(result);
            m_PanelFailure?.gameObject.SetActive(!result);
        }

        private void CheckLevelConditions()
        {
            int numCompleted = 0;

            for (int i = 0; i < m_Conditions.Length; i++)
            {
                if (m_Conditions[i].IsCompleted == true)
                {
                    numCompleted++;
                }
            }

            if (numCompleted == m_Conditions.Length)
            {
                m_IsLevelCompleted = true;
                StopLevelActivity();
                MapCompletion.SaveEpisodeResult(levelScore);
                Show(m_IsLevelCompleted);
            }
        }

        protected void Lose()
        {
            StopLevelActivity();
            Show(false);
        }

        public void OnPlayNext()
        {
            LevelSequenceController.Instance.AdvanceLevel();
        }

        public void OnRestartLevel()
        {
            LevelSequenceController.Instance.RestartLevel();
        }

        public void OnExitToMenu()
        {
            LevelSequenceController.Instance.ExitToMenu();
        }

        private void StopLevelActivity()
        {
            foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
            {
                enemy.GetComponent<CharacterMotor>().enabled = false;
                enemy.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            }

            void DisableAll<T>() where T : MonoBehaviour
            {
                foreach (var component in FindObjectsByType<T>(FindObjectsSortMode.None))
                {
                    component.enabled = false;
                }
            }

            DisableAll<Spawner>();
            DisableAll<Projectile>();
            DisableAll<Tower>();
        }
    }
}
