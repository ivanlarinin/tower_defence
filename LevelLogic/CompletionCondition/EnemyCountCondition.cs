using UnityEngine;

namespace TowerDefence
{
    public class EnemyCountCondition : LevelCondition
    {
        override public bool IsCompleted => FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length == 0;
    }
}