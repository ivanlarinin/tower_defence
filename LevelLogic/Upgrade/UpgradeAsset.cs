using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeAsset", menuName = "Scriptable Objects/UpgradeAsset")]
public class UpgradeAsset : ScriptableObject
{
    public enum UpgradeType
    {
        ExtraLife,
        DamageBoost,
        TimeAbility,
        FireAbility
    }
    public UpgradeType upgradeType;

    public Sprite sprite;
    public int[] costByLevel = new int[] { 2, 4, 6 };
}
