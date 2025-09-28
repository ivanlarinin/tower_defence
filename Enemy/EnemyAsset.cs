using UnityEngine;

/// <summary>
/// Enemy asset data container.
/// </summary>
namespace TowerDefence
{
    [CreateAssetMenu(fileName = "EnemyAsset", menuName = "TowerDefence/EnemyAsset", order = 0)]
    public sealed class EnemyAsset : ScriptableObject
    {
        [Header("Visuals")]
        [field: SerializeField] public Color color = Color.white;
        [field: SerializeField] public RuntimeAnimatorController animations;
        [field: SerializeField] public int damage = 1;
        [field: SerializeField] public int gold = 1; 
        [field: SerializeField] public int buff = 0;

        [field: SerializeField] public float Radius { get; private set; }


        // [field: SerializeField] public int Health { get; private set; }
        // [field: SerializeField] public int Reward { get; private set; }

        [Header("Stats")]
        [field: SerializeField] public float Speed { get; private set; }

         
    }
}