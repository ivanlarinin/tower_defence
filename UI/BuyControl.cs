using UnityEngine;

namespace TowerDefence
{
    public class BuyControl : MonoBehaviour
    {
        private RectTransform t;
        private void Awake()
        {
            t = GetComponent<RectTransform>();
            BuildSite.OnClickEvent += MoveToTransform;
            gameObject.SetActive(false);
        }

        private void MoveToTransform(Transform target)
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
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
