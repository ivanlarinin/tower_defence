using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    ///  Control for buying towers, appears when clicking on a build site
    /// </summary>
    public class BuyControl : MonoBehaviour
    {
        [SerializeField] private TowerBuyControl m_TowerBuyPrefab;
        [SerializeField] private float m_Radius = 80f;
        [SerializeField] private float m_StartAngle = 90f;
        private List<TowerBuyControl> m_ActiveControl;
        private RectTransform t;
        private void Awake()
        {
            t = GetComponent<RectTransform>();
            BuildSite.OnClickEvent += MoveToBuildSite;
            m_ActiveControl = new List<TowerBuyControl>();
            gameObject.SetActive(false);
        }

        private void MoveToBuildSite(BuildSite target)
        {
            if (target)
            {
                Vector2 localPoint;
                var canvas = t.GetComponentInParent<Canvas>();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    Camera.main.WorldToScreenPoint(target.transform.root.position),
                    canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null,
                    out localPoint
                );

                t.anchoredPosition = localPoint;
                foreach (var control in m_ActiveControl) Destroy(control.gameObject);
                m_ActiveControl = new List<TowerBuyControl>();

                foreach (var asset in target.BuildableTowers)
                {
                    if (asset.IsAvailible())
                    {
                        var newControl = Instantiate(m_TowerBuyPrefab, transform);
                        m_ActiveControl.Add(newControl);
                        newControl.SetTowerAsset(asset);
                    }
                }

                // arrange in circle
                int count = m_ActiveControl.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        float angleDeg = m_StartAngle + (360f * i) / count;
                        float rad = angleDeg * Mathf.Deg2Rad;
                        Vector2 pos = new Vector2(Mathf.Cos(rad) * m_Radius, Mathf.Sin(rad) * m_Radius);

                        var rt = m_ActiveControl[i].GetComponent<RectTransform>();
                        if (rt != null)
                        {
                            rt.anchoredPosition = pos;
                        }
                    }

                    gameObject.SetActive(true);
                    foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
                    {
                        tbc.SetBuildSite(target.transform.root);
                    }
                }
            }
            else
            {
                foreach (var control in m_ActiveControl) Destroy(control.gameObject);
                m_ActiveControl.Clear();
                gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            BuildSite.OnClickEvent -= MoveToBuildSite;
        }
    }
}
