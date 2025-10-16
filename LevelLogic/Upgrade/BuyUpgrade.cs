using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    /// <summary>
    /// Handles the logic for displaying and purchasing upgrades in the game.
    /// </summary>
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeAsset m_Asset;
        [SerializeField] private Button m_UpgradeButton;
        [SerializeField] private Text m_Cost;
        [SerializeField] private Text m_Level;
        [SerializeField] private Image m_Icon;

        /// <summary>
        /// Gets the cost of the current upgrade level.
        /// </summary>
        public int GetCurrentUpgradeCost()
        {
            int savedLevel = Upgrades.GetUpgradeLevel(m_Asset);
            if (savedLevel >= m_Asset.costByLevel.Length)
            {
                return -1; // Indicates max level (no cost available).
            }
            return m_Asset.costByLevel[savedLevel];
        }

        /// <summary>
        /// Initializes the upgrade UI with the current upgrade level and cost.
        /// </summary>
        public void Initialize()
        {
            m_Icon.sprite = m_Asset.sprite;

            int savedLevel = Upgrades.GetUpgradeLevel(m_Asset);
            if (savedLevel >= m_Asset.costByLevel.Length)
            {
                m_Level.text = $"Level {savedLevel}";
                m_Cost.text = "MAX";
                m_UpgradeButton.interactable = false;
                return;
            }
            int cost = GetCurrentUpgradeCost();
            var t = m_Asset.GetType();
            if (t == typeof(FireAbility))
            {
                m_Level.text = "Fireball";
                m_Cost.text = $"Buy: {cost}";
            }
            else
            {
                m_Level.text = $"Level {savedLevel + 1}";
                m_Cost.text = $"Buy: {cost}";
            }

            m_UpgradeButton.interactable = true;
        }

        /// <summary>
        /// Attempts to purchase the upgrade. Updates the UI if the purchase is successful.
        /// </summary>
        public void Buy()
        {
            if (UpgradeShop.Instance.TryBuyUpgrade(m_Asset))
            {
                Initialize();
            }
            else
            {
                Debug.Log("[BuyUpgrade] Purchase failed (not enough money or max level)");
            }
        }
    }
}