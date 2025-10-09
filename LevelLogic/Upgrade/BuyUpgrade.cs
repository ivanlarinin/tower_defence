using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeAsset asset;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Text cost, level;
        [SerializeField] private Image icon;

        public void Initialize()
        {
            icon.sprite = asset.sprite;
            var savedLevel = Upgrades.GetUpgradeLevel(asset);
            this.level.text = $"Level {savedLevel + 1}";
            cost.text = asset.costByLevel[savedLevel].ToString();
            if (savedLevel >= asset.costByLevel.Length)
            {
                cost.text = "MAX";
                upgradeButton.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                cost.text = asset.costByLevel[savedLevel].ToString();
            }
        }

        public void Buy()
        {
            Upgrades.BuyUpgrade(asset);
        }
    }
}
