using UnityEngine;

namespace TowerDefence
{
    public class MapLevell : MonoBehaviour
    {
        [SerializeField] private Episode episode;
        public void LoadLevel()
        {
            if (episode == null) return;
            LevelSequenceController.Instance.StartEpisode(episode);
        }
    }
}