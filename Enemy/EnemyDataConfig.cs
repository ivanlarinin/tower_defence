using System;
using System.Collections.Generic;

[Serializable]
public class SquadData
{
    public string asset;
    public int count;
}

[Serializable]
public class GroupData
{
    public int pathIndex;
    public List<SquadData> squads = new List<SquadData>();
}

[Serializable]
public class WaveData
{
    public float prepareTime = 10f;
    public List<GroupData> groups = new List<GroupData>();
    public int nextWave = -1;
}

[Serializable]
public class WaveConfig
{
    public List<WaveData> waves = new List<WaveData>();
}
