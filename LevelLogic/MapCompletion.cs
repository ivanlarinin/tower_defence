using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    public class MapCompletion : SingletonBase<MapCompletion>
    {
        public const string Filename = "map_completion";

        private Dictionary<Episode, EpisodeScore> _scoreLookup = new();
        private int totalScore;

        public int TotalScore => totalScore;

        [Serializable]
        public class EpisodeScore
        {
            public Episode Episode;
            public int BestScore;
        }

        protected override void Awake()
        {
            base.Awake();

            DiscoverAllEpisodes();

            // Load saved data
            if (Saver<EpisodeScore[]>.TryLoad(Filename, out var loadedData))
            {
                foreach (var saved in loadedData)
                {
                    if (_scoreLookup.TryGetValue(saved.Episode, out var local))
                        local.BestScore = saved.BestScore;
                }
            }

            SaveAllEpisodes();
            UpdateTotalScore();
        }

        private void DiscoverAllEpisodes()
        {
            var levels = FindObjectsByType<MapLevel>(FindObjectsSortMode.None);
            foreach (var level in levels)
            {
                var episode = level.Episode;
                if (!_scoreLookup.ContainsKey(episode))
                    _scoreLookup[episode] = new EpisodeScore { Episode = episode, BestScore = 0 };
            }
        }

        private void UpdateTotalScore()
        {
            totalScore = 0;
            foreach (var score in _scoreLookup.Values)
                totalScore += score.BestScore;
        }

        private void SaveAllEpisodes()
        {
            Saver<EpisodeScore[]>.Save(Filename, new List<EpisodeScore>(_scoreLookup.Values).ToArray());
        }

        public static void SaveEpisodeResult(Episode episode, int levelScore)
        {
            if (Instance._scoreLookup.TryGetValue(episode, out var score) && levelScore > score.BestScore)
            {
                score.BestScore = levelScore;
                Instance.SaveAllEpisodes();
                Instance.UpdateTotalScore();
            }
        }

        public int GetEpisodeScore(Episode episode)
        {
            return _scoreLookup.TryGetValue(episode, out var score) ? score.BestScore : 0;
        }
    }
}
