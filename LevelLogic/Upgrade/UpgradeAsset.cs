using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeAsset", menuName = "Scriptable Objects/UpgradeAsset")]
public class UpgradeAsset : ScriptableObject
{
    public Sprite sprite;
    public int[] costByLevel = new int[] { 100, 200, 300 };
}
