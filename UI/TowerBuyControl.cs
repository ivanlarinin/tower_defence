using TowerDefence;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class TowerBuyControl : MonoBehaviour
    {
        [SerializeField] private TowerAssets m_towerAsset;
        public void SetTowerAsset(TowerAssets assets) { m_towerAsset = assets; }
        [SerializeField] private Text m_text;
        [SerializeField] private Button m_button;
        [SerializeField] private Transform buildSite;
        public void SetBuildSite(Transform value)
        {
            buildSite = value;
        }

        private void Start()
        {
            TDPlayer.GoldUpdateSubscribe(GoldStatusCheck);
            m_text.text = m_towerAsset.goldCost.ToString();
            m_button.GetComponent<Image>().sprite = m_towerAsset.GUIsprite;
        }

        private void GoldStatusCheck(int gold)
        {
            if (gold >= m_towerAsset.goldCost != m_button.interactable)
            {
                m_button.interactable = !m_button.interactable;
                m_text.color = m_button.interactable ? Color.white : Color.red;
            }
        }

        public void BuyTower()
        {
            print(m_towerAsset.GUIsprite);
            TDPlayer.Instance.TryBuild(m_towerAsset, buildSite);
            BuildSite.HideControls();
        }
    }
}