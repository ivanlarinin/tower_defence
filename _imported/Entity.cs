using UnityEngine;

/// <summary>
/// Base class for all interactive game objects in the scene.
/// </summary>
public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    private string m_Nickname;
    public string Nickname => m_Nickname;
}