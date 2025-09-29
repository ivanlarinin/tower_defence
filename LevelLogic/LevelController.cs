using UnityEngine;
using UnityEngine.Events;

namespace TowerDefence
{
    public interface ILevelCondition
    {
        /// <summary>
        /// True если условие выполнено.
        /// </summary>
        bool IsCompleted { get; }
    }
    public class LevelController : SingletonBase<LevelController>
    {
        public event UnityAction LevelPassed;
        public event UnityAction LevelLost;

        [SerializeField] private LevelCondition[] m_Conditions;

        [SerializeField] private GameObject m_PanelSuccess;
        [SerializeField] private GameObject m_PanelFailure;

        private bool m_IsLevelCompleted;
        private float m_LevelTime;

        // public bool HasNextLevel => m_LevelProperties.NextLevel != null;
        public float LevelTime => m_LevelTime;

        #region Unity events

        private void Start()
        {
            m_Conditions = GetComponentsInChildren<LevelCondition>();
        }

        private void Update()
        {
            if (m_IsLevelCompleted == false)
            {
                m_LevelTime += Time.deltaTime;
                CheckLevelConditions();
            }
        }

        #endregion

        public void Show(bool result)
        {
            if (result)
            {
                // UpdateCurrentLevelStats();
                // UpdateVisualStats();
            }

            m_PanelSuccess?.gameObject.SetActive(result);
            // m_PanelFailure?.gameObject.SetActive(!result);
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
                Show(m_IsLevelCompleted);

            }
        }

        protected void Lose()
        {
            LevelLost?.Invoke();
            Time.timeScale = 0;
        }

        public void OnPlayNext()
        {
            LevelSequenceController.Instance.AdvanceLevel();
        }

        public void OnRestartLevel()
        {
            LevelSequenceController.Instance.RestartLevel();
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
