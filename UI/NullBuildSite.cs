using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefence
{
    public class NullBuildSite : BuildSite
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            HideControls();
        }
    }
}
