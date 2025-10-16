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
        [SerializeField] private UpgradeAsset requiredUpgrade;
        [SerializeField] private int requiredUpgradeLevel;
        public bool IsAvailible() => !requiredUpgrade ||
            requiredUpgradeLevel <= Upgrades.GetUpgradeLevel(requiredUpgrade);

        public TowerAssets[] m_UpgradesTo;
    }
}