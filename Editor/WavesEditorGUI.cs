using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class WaveEditorWindow : EditorWindow
{
    private List<string> enemyAssets = new List<string> { "Enemy_01", "Enemy_02", "Enemy_03" };

    private int currentWave = 0;
    private int currentGroup = 0;
    private int enemyOneCount = 0;
    private int enemyTwoCount = 0;
    private int enemyThreeCount = 0;
    private float prepareTime = 10f;
    private Vector2 scrollPos;

    private WaveConfig config;
    private string jsonPath;

    [MenuItem("TowerDefense/Wave Editor")]
    public static void ShowWindow()
    {
        GetWindow<WaveEditorWindow>("Wave Editor");
    }

    private void OnEnable()
    {
        jsonPath = Path.Combine(Application.dataPath, "waves.json");
        LoadJson();
    }

    private void OnGUI()
    {
        GUILayout.Label("Add/Edit Enemy Squad", EditorStyles.boldLabel);

        currentWave = EditorGUILayout.IntField("Current Wave", currentWave);
        currentGroup = EditorGUILayout.IntField("Current Group", currentGroup);
        prepareTime = EditorGUILayout.FloatField("Wait Time (seconds)", prepareTime);

        GUILayout.Space(10);

        enemyOneCount = EditorGUILayout.IntField("Light Enemy Count", enemyOneCount);
        enemyTwoCount = EditorGUILayout.IntField("Heavy Enemy Count", enemyTwoCount);
        enemyThreeCount = EditorGUILayout.IntField("Mage Enemy Count", enemyThreeCount);

        GUILayout.Space(10);

        if (GUILayout.Button("Add/Edit Wave"))
        {
            AddOrEditWave();
        }

        if (GUILayout.Button("Save JSON"))
        {
            SaveJson();
        }

        if (GUILayout.Button("Load JSON"))
        {
            LoadJson();
        }

        GUILayout.Space(20);
        GUILayout.Label("Waves Overview", EditorStyles.boldLabel);

        if (config != null && config.waves.Count > 0)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));

            for (int w = 0; w < config.waves.Count; w++)
            {
                WaveData wave = config.waves[w];
                EditorGUILayout.LabelField($"Wave {w} (Wait: {wave.prepareTime}s)");

                for (int g = 0; g < wave.groups.Count; g++)
                {
                    GroupData group = wave.groups[g];
                    string squadText = "";

                    foreach (var squad in group.squads)
                    {
                        string shortName = "L"; // default light
                        switch (squad.asset)
                        {
                            case "Enemy_01": shortName = "L"; break;
                            case "Enemy_02": shortName = "H"; break;
                            case "Enemy_03": shortName = "M"; break;
                        }

                        squadText += $"{shortName}:{squad.count} ";
                    }

                    EditorGUILayout.LabelField($"  Group {g}: {squadText}");
                }
            }

            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.LabelField("No waves loaded.");
        }

    }

    private void AddOrEditWave()
    {
        while (config.waves.Count <= currentWave)
        {
            config.waves.Add(new WaveData());
        }

        WaveData wave = config.waves[currentWave];
        wave.prepareTime = prepareTime; // save wait time

        while (wave.groups.Count <= currentGroup)
        {
            wave.groups.Add(new GroupData());
        }

        GroupData group = wave.groups[currentGroup];
        group.pathIndex = currentGroup; // example

        group.squads.Clear();
        if (enemyOneCount > 0)
            group.squads.Add(new SquadData { asset = enemyAssets[0], count = enemyOneCount });
        if (enemyTwoCount > 0)
            group.squads.Add(new SquadData { asset = enemyAssets[1], count = enemyTwoCount });
        if (enemyThreeCount > 0)
            group.squads.Add(new SquadData { asset = enemyAssets[2], count = enemyThreeCount });

        Debug.Log($"Wave {currentWave}, Group {currentGroup} updated with wait time {prepareTime}s.");
    }


    private void SaveJson()
    {
        string json = JsonUtility.ToJson(config, true);
        File.WriteAllText(jsonPath, json);
        Debug.Log($"Saved JSON to {jsonPath}");
    }

    private void LoadJson()
    {
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            config = JsonUtility.FromJson<WaveConfig>(json);
            Debug.Log("Loaded waves.json");
        }
        else
        {
            config = new WaveConfig();
            Debug.Log("No existing JSON found. Created new config.");
        }

        PopulateGuiFields(); // sync GUI with loaded data
    }

    private void PopulateGuiFields()
    {
        if (config.waves.Count > currentWave)
        {
            WaveData wave = config.waves[currentWave];
            prepareTime = wave.prepareTime; // populate wait time

            if (wave.groups.Count > currentGroup)
            {
                GroupData group = wave.groups[currentGroup];
                enemyOneCount = 0;
                enemyTwoCount = 0;
                enemyThreeCount = 0;

                foreach (var squad in group.squads)
                {
                    switch (squad.asset)
                    {
                        case "Enemy_01": enemyOneCount = squad.count; break;
                        case "Enemy_02": enemyTwoCount = squad.count; break;
                        case "Enemy_03": enemyThreeCount = squad.count; break;
                    }
                }
            }
            else
            {
                enemyOneCount = 0;
                enemyTwoCount = 0;
                enemyThreeCount = 0;
            }
        }
        else
        {
            prepareTime = 10f;
            enemyOneCount = 0;
            enemyTwoCount = 0;
            enemyThreeCount = 0;
        }
    }


}
