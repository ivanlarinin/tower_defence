using System;
using UnityEngine;

namespace TowerDefence
{
    public class MapCompletion : SingletonBase<MapCompletion>
    {
        public const string Filename = "map_completion";
        [SerializeField] private EpisodeScore[] m_CompletionData;

        [Serializable]
        private class EpisodeScore
        {
            public Episode Episode;
            public int BestScore;
        }

        public static void SaveEpisodeResult(int levelScore)
        {
            Instance.SaveResult(LevelSequenceController.Instance.CurrentEpisode, levelScore);
        }

        private void SaveResult(Episode currentEpisode, int levelScore)
        {
            foreach (var episodeScore in m_CompletionData)
            {
                if (episodeScore.Episode == currentEpisode)
                {
                    if (levelScore > episodeScore.BestScore)
                    {
                        episodeScore.BestScore = levelScore;
                        Saver<EpisodeScore[]>.Save(Filename, m_CompletionData);
                    }
                    return;
                }
            }
        }

        private new void Awake()
        {
            base.Awake();

            if (Saver<EpisodeScore[]>.TryLoad(Filename, out var loadedData))
            {
                for (int i = 0; i < m_CompletionData.Length; i++)
                {
                    for (int j = 0; j < loadedData.Length; j++)
                    {
                        if (m_CompletionData[i].Episode == loadedData[j].Episode)
                        {
                            m_CompletionData[i].BestScore = loadedData[j].BestScore;
                            break;
                        }
                    }
                }
            }
        }

        public bool TryIndex(int id, out Episode episode, out int score)
        {
            if (id >= 0 && id < m_CompletionData.Length)
            {
                episode = m_CompletionData[id].Episode;
                score = m_CompletionData[id].BestScore;
                return true;
            }
            episode = null;
            score = 0;
            return false;
        }
    }
}
