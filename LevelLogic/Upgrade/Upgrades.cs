using UnityEngine;

namespace TowerDefence
{
    public class Upgrades : SingletonBase<Upgrades>
    {
        public const string filename = "upgrades.dat";

        [System.Serializable]
        private class UpgradeSave
        {
            public UpgradeAsset asset;
            public int level = 0;
        }
        [SerializeField] private UpgradeSave[] save;

        private new void Awake()
        {
            base.Awake();

            if (!Saver<UpgradeSave[]>.TryLoad(filename, out save) || save == null || save.Length == 0)
            {
                Debug.Log("[Upgrades] No save found — initializing from UpgradeAssets");

                // find all UpgradeAsset files (assuming they live in a Resources folder)
                var allAssets = Resources.LoadAll<UpgradeAsset>("");

                save = new UpgradeSave[allAssets.Length];
                for (int i = 0; i < allAssets.Length; i++)
                {
                    save[i] = new UpgradeSave
                    {
                        asset = allAssets[i],
                        level = 0
                    };
                }

                Saver<UpgradeSave[]>.Save(filename, save);
                Debug.Log($"[Upgrades] Created new upgrades.dat with {save.Length} entries");
            }
            else
            {
                Debug.Log($"[Upgrades] Loaded {save.Length} upgrades from file");
            }
        }

        public static void BuyUpgrade(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.save)
            {
                if (upgrade.asset == asset)
                {
                    upgrade.level += 1;
                    Saver<UpgradeSave[]>.Save(filename, Instance.save);
                }
            }
        }

        public static int GetUpgradeLevel(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.save)
            {
                if (upgrade.asset == asset)
                {
                    return upgrade.level;
                }
            }
            return 0;
        }

        public void ApplyUpgradesToPlayer(TDPlayer player)
        {
            foreach (var upgrade in save)
            {
                switch (upgrade.asset.upgradeType)
                {
                    case UpgradeAsset.UpgradeType.ExtraLife:
                        player.AddLife(upgrade.level);
                        break;

                    case UpgradeAsset.UpgradeType.DamageBoost:
                        // player.AddDamage(upgrade.level);
                        break;

                    case UpgradeAsset.UpgradeType.TimeAbility:
                        // player.AddSpeed(upgrade.level);
                        break;

                    case UpgradeAsset.UpgradeType.FireAbility:
                        // player.AddFireAbility(upgrade.level);
                        break;
                }
            }
        }
        public static int GetUpgradeLevel(UpgradeAsset.UpgradeType type)
        {
            foreach (var upgrade in Instance.save)
            {
                if (upgrade.asset.upgradeType == type)
                    return upgrade.level;
            }
            return 0;
        }


    }
}
