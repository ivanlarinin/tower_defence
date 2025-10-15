using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField] private int money;
        [SerializeField] private Text moneyText;
        [SerializeField] private BuyUpgrade[] upgrades;

        public static UpgradeShop Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            money = MapCompletion.Instance.TotalScore;
            moneyText.text = money.ToString();
            foreach (var upgrade in upgrades)
            {
                upgrade.Initialize();
            }
        }

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
            money = MapCompletion.Instance.TotalScore;
            moneyText.text = money.ToString();

            Upgrades.BuyUpgrade(asset);
            return true;
        }
    }
}