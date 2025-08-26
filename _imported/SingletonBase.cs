using UnityEngine;

/// <summary>
/// Base class for creating singleton MonoBehaviour classes.
/// Ensures only one instance exists in the scene.
/// </summary>
[DisallowMultipleComponent]
public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    // The single instance of this class
    public static T Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance.
    /// If another instance exists, this one will be destroyed.
    /// </summary>
    public void Init()
    {
        if (Instance != null)
        {
            Debug.LogWarning("MonoSingleton: object of type already exists, instance will be destroyed - " + typeof(T).Name);
            Destroy(this);
            return;
        }

        Instance = this as T;
    }
}
