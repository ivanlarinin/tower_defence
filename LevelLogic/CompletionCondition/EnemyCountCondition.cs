using UnityEngine;

namespace TowerDefence
{
    public class EnemyCountCondition : LevelCondition
    {
        private bool _isCompleted;

        void Start()
        {
            FindObjectsByType<EnemyWavesManager>(FindObjectsInactive.Include, FindObjectsSortMode.None)[0].OnAllWavesCompleted += () => _isCompleted = true;
        }

        public override bool IsCompleted => _isCompleted;
    }
}
