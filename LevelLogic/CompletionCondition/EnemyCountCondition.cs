using UnityEngine;

namespace TowerDefence
{
    public class EnemyCountCondition : LevelCondition
    {
        private bool enemyCount;

        void Update()
        {
            enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length == 0;
        }

        public override bool IsCompleted => enemyCount;
    }
}
