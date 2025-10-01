using UnityEngine;
using UnityEngine.UI;
namespace TowerDefence
{
    public class MapLevel : MonoBehaviour
    {
        private Episode m_episode;
        [SerializeField] private Text text;
        public void LoadLevel()
        {
            if (m_episode == null) return;
            LevelSequenceController.Instance.StartEpisode(m_episode);
        }
        public void SetLevelData(Episode episode, int score)
        {
            m_episode = episode;
            text.text = $"{score}/3";
        }
    }
}