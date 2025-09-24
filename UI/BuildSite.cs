using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace TowerDefence
{
    public class BuildSite : MonoBehaviour, IPointerDownHandler
    {
        public static event Action<Transform> OnClickEvent;

        protected void InvokeNullEvent()
        {
            OnClickEvent?.Invoke(null);
        }
        
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnClickEvent?.Invoke(transform.root);
        }
    }
}
