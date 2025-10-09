using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
namespace TowerDefence
{
    public class MapLevel : MonoBehaviour
    {
        private Episode m_episode;
        private RectTransform resultPanel;
        [SerializeField] private Text text;

        // If self is active
        public bool IsComplete
        {
            get
            {
                return gameObject.activeSelf.Equals(true) && resultPanel.gameObject.activeSelf.Equals(true);

            }
        }
        
        public void LoadLevel()
        {
            if (m_episode == null) return;
            LevelSequenceController.Instance.StartEpisode(m_episode);
        }
        public void SetLevelData(Episode episode, int score)
        {
            m_episode = episode;
            resultPanel.GameObject().SetActive(score > 0);
            text.text = $"{score}/3";
        }

        public void Initialize()
        {
            var score = MapCompletion.Instance.GetEpisodeScore(m_episode);
            resultPanel.gameObject.SetActive(score > 0);
            for (int i = 0; i < score; i++)
            {
                var child = transform.GetChild(i);
                if (child.name.StartsWith("Star"))
                {
                    // resultPanel[i].color = Color.white;
                }
            }
        }
    }
}