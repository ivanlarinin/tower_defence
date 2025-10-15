using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeAsset asset;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Text cost;
        [SerializeField] private Text level;
        [SerializeField] private Image icon;

        public void Initialize()
        {
            icon.sprite = asset.sprite;

            int savedLevel = Upgrades.GetUpgradeLevel(asset);
            if (savedLevel >= asset.costByLevel.Length)
            {
                level.text = $"Level {savedLevel}";
                cost.text = "MAX";
                upgradeButton.interactable = false;
                return;
            }

            level.text = $"Level {savedLevel + 1}";
            cost.text = asset.costByLevel[savedLevel].ToString();
            upgradeButton.interactable = true;
        }

        public void Buy()
        {
            if (UpgradeShop.Instance.TryBuyUpgrade(asset))
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
