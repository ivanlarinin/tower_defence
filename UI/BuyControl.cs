using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    public class BuyControl : MonoBehaviour
    {
        [SerializeField] private TowerBuyControl m_TowerBuyPrefab;
        [SerializeField] private TowerAssets[] m_towerAssets;
        [SerializeField] private UpgradeAsset mageTowerUpgrade;
        private List<TowerBuyControl> m_ActiveControl;
        private RectTransform t;
        private void Awake()
        {
            t = GetComponent<RectTransform>();
            BuildSite.OnClickEvent += MoveToBuildSite;
            gameObject.SetActive(false);
        }

        private void MoveToBuildSite(Transform target)
        {
            if (target)
            {
                Vector2 localPoint;
                var canvas = t.GetComponentInParent<Canvas>();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    Camera.main.WorldToScreenPoint(target.position),
                    canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null,
                    out localPoint
                );
                t.anchoredPosition = localPoint;
                gameObject.SetActive(true);
                m_ActiveControl = new List<TowerBuyControl>();
                for (int i = 0; i<m_towerAssets.Length; i++)
                {
                    if (i != 1 || Upgrades.GetUpgradeLevel(mageTowerUpgrade) > 0)
                    {
                        newControl.SetTowerAsset(m_towerAssets[i]);
                        var newControl = Instantiate(m_TowerBuyPrefab, transform);
                        m_ActiveControl.Add(newControl);
                        newControl.transform.position += Vector3.left * 80 * i;
                    }
                }
            }
            else
            {
                foreach (var control in m_ActiveControl) Destroy(control.gameObject);
                gameObject.SetActive(false);
            }
            
            foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
            {
                tbc.SetBuildSite(target);
            } 
        }
    }
}
