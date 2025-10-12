using UnityEngine;

namespace TowerDefence
{
    public class LevelDisplayController : MonoBehaviour
    {
        private MapLevel[] levels;
        private BranchLevel[] branchLevels;

        private void Start()
        {
            levels = FindObjectsByType<MapLevel>(FindObjectsSortMode.None);
            branchLevels = FindObjectsByType<BranchLevel>(FindObjectsSortMode.None);

            InitializeLevels();
            InitializeBranchLevels();
        }

        private void InitializeLevels()
        {
            foreach (var level in levels)
                level.Initialize();
        }

        private void InitializeBranchLevels()
        {
            foreach (var branch in branchLevels)
                branch.TryActivate();
        }
    }
}
