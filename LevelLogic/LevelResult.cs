using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    /// <summary>
    /// Manages HUD panels for win and lose conditions.
    /// </summary>
    public class LevelResult : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private Image resultImage;
        [SerializeField] private Button resultButton;

        [Header("Result Sprites")]
        [SerializeField] private Sprite winSprite;
        [SerializeField] private Sprite loseSprite;

        [Header("Sound Effects")]
        [SerializeField] private Sound m_WinSound;
        [SerializeField] private Sound m_LoseSound;

        private void Awake()
        {
            // Safety checks
            if (resultPanel == null || resultImage == null || resultButton == null)
            {
                Debug.LogError("LevelResult: Assign all references in the inspector!");
                return;
            }

            // Initially hide the panel
            resultPanel.SetActive(false);

            // Setup button listener
            resultButton.onClick.RemoveAllListeners();
            resultButton.onClick.AddListener(LevelController.Instance.OnPlayNext);
        }

        /// <summary>
        /// Shows the result panel with the appropriate sprite
        /// </summary>
        /// <param name="result">true = Win, false = Lose</param>
        public void ShowResult(bool result)
        {
            resultPanel.SetActive(true);

            resultImage.sprite = result ? winSprite : loseSprite;

            if (result)
            {
                m_WinSound.Play();
            }
            else
            {
                m_LoseSound.Play();
            }
        }

        /// <summary>
        /// Hides the result panel
        /// </summary>
        public void HideAll()
        {
            resultPanel.SetActive(false);
        }
    }
}
