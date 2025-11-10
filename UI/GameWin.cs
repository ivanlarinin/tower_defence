using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// Manages the Game Win UI element based on player's collected score.
    /// </summary>
    public class GameWin : MonoBehaviour
    {
        [SerializeField]
        private GameObject winPanel;

        private void Start()
        {
            var map = MapCompletion.Instance;
            if (map == null)
            {
                Debug.LogWarning("MapCompletion.Instance is null.");
                gameObject.SetActive(false);
                return;
            }

            // read collected score
            int collected = map.TotalScore;

            if (collected == 12)
            {
                winPanel.SetActive(true);
            }
            else
            {
                winPanel.SetActive(false);
            }

        }
    }
}