using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class MapLevel : MonoBehaviour
    {
        [SerializeField] private Episode m_episode;
        [SerializeField] private MapLevel m_prevEpisode;
        [SerializeField] private RectTransform resultPanel;
        [SerializeField] private Image[] stars;

        public Episode Episode => m_episode;

        public bool IsComplete => MapCompletion.Instance.GetEpisodeScore(m_episode) == 3;

        public void LoadLevel()
        {
            if (m_episode != null)
                LevelSequenceController.Instance.StartEpisode(m_episode);
        }

        public void Initialize()
        {
            int score = MapCompletion.Instance.GetEpisodeScore(m_episode);
            // resultPanel.gameObject.SetActive(score > 0);

            for (int i = 0; i < stars.Length; i++)
                stars[i].gameObject.SetActive(i < score);

            if (m_prevEpisode != null)
            {
                bool unlocked = MapCompletion.Instance.GetEpisodeScore(m_prevEpisode.Episode) > 0;
                gameObject.SetActive(unlocked);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }
}
