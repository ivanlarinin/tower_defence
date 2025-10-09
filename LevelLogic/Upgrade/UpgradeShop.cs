using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField] private int money;
        [SerializeField] private Text moneyText;
        [SerializeField] private BuyUpgrade[] upgrades;
        private void Start()
        {
            money = MapCompletion.Instance.TotalScore;
            moneyText.text = money.ToString();
            foreach (var upgrade in upgrades)
            {
                upgrade.Initialize();
            }
        }
    }
}