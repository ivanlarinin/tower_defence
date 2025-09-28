using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu]
    public class TowerAssets : ScriptableObject
    {
        public int goldCost = 15;
        public Sprite sprite;
        public Sprite GUIsprite;
        public TurretProperties turretProperties;
    }
}