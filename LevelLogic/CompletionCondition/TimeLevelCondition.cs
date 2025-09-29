using UnityEngine;

public class TimeLevelCondition : LevelCondition
{
    [SerializeField] private float timeLimit = 4f;
    void Start()
    {
        timeLimit += Time.time;
    }
    override public bool IsCompleted => Time.time > timeLimit;
}
