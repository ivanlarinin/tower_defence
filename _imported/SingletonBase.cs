using UnityEngine;

/// <summary>
/// Base class for creating singleton MonoBehaviour classes.
/// Ensures only one instance exists in the scene.
/// </summary>
[DisallowMultipleComponent]
public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// Automatically mark object as persistent.
    /// </summary>
    [Header("Singleton")]
    [SerializeField] private bool m_DoNotDestroyOnLoad;

    /// <summary>
    /// Singleton instance. May be null if DoNotDestroyOnLoad flag was not set.
    /// </summary>
    public static T Instance { get; private set; }

    #region Unity events

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("MonoSingleton: object of type already exists, instance will be destroyed=" + typeof(T).Name);
            Destroy(this);
            return;
        }

        Instance = this as T;

        if (m_DoNotDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }

    #endregion
}
