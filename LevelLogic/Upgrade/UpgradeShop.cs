using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    /// <summary>
    /// Manages the upgrade shop, including available upgrades and player money.
    /// </summary>
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField] private int m_Money;
        [SerializeField] private Text m_MoneyText;
        [SerializeField] private BuyUpgrade[] m_Upgrades;

        /// <summary>
        /// Singleton instance of the UpgradeShop.
        /// </summary>
        public static UpgradeShop Instance { get; private set; }

        /// <summary>
        /// Sets up the singleton instance.
        /// </summary>
        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Initializes the shop with the player's total score and updates the UI.
        /// </summary>
        private void Start()
        {
            m_Money = MapCompletion.Instance.TotalScore;
            m_MoneyText.text = m_Money.ToString();
            foreach (var upgrade in m_Upgrades)
            {
                upgrade.Initialize();

                if (upgrade.GetCurrentUpgradeCost() > m_Money)
                {
                    upgrade.gameObject.SetActive(false);
                    m_MoneyText.color = Color.red;
                }
                else
                {
                    upgrade.gameObject.SetActive(true);
                    m_MoneyText.color = Color.white;
                }
            }
        }

        /// <summary>
        /// Attempts to purchase an upgrade for the given asset.
        /// </summary>
        /// <param name="asset">The upgrade asset to purchase.</param>
        /// <returns>True if the purchase was successful, false otherwise.</returns>
        public bool TryBuyUpgrade(UpgradeAsset asset)
        {
            int currentLevel = Upgrades.GetUpgradeLevel(asset);

            if (currentLevel >= asset.costByLevel.Length)
            {
                Debug.Log($"[UpgradeShop] {asset.name} is maxed out");
                return false;
            }

            int cost = asset.costByLevel[currentLevel];
            if (!MapCompletion.Instance.TrySpendScore(cost))
            {
                Debug.Log($"[UpgradeShop] Not enough money for {asset.name}. Need {cost}, have {MapCompletion.Instance.TotalScore}");
                return false;
            }

            m_Money = MapCompletion.Instance.TotalScore;
            m_MoneyText.text = m_Money.ToString();

            Upgrades.BuyUpgrade(asset);
            return true;
        }
    }
}