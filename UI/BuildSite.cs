using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace TowerDefence
{
    /// <summary>
    /// Holds a list of towers that can be built here and
    /// raises click events for UI handling.
    /// </summary>
    public class BuildSite : MonoBehaviour, IPointerDownHandler
    {
        #region Fields

        [SerializeField] private TowerAssets[] m_BuildableTowers;

        #endregion

        #region Events

        /// <summary>
        /// Raised when a build site is clicked. Passes the clicked BuildSite or null to hide controls.
        /// </summary>
        public static event Action<BuildSite> OnClickEvent;

        #endregion

        #region Public Methods

        /// <summary>
        /// Exposes the buildable tower assets for this build site.
        /// </summary>
        public TowerAssets[] BuildableTowers => m_BuildableTowers;

        /// <summary>
        /// Sets the towers that can be built on this site. If the provided list is null or empty,
        /// the build site's parent GameObject will be destroyed.
        /// </summary>
        /// <param name="towers">Array of buildable tower assets.</param>
        public void SetBuildableTowers(TowerAssets[] towers)
        {
            if (towers == null || towers.Length == 0)
            {
                Destroy(transform.parent.gameObject);
                return;
            }

            m_BuildableTowers = towers;
        }

        /// <summary>
        /// Hides build controls by invoking the OnClickEvent with null.
        /// </summary>
        public static void HideControls()
        {
            OnClickEvent?.Invoke(null);
        }

        #endregion

        #region Unity Methods

        /// <summary>
        /// Invoked by the event system when this build site is clicked.
        /// </summary>
        /// <param name="eventData">Pointer event data.</param>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnClickEvent?.Invoke(this);
        }

        #endregion
    }
}
