using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System;

public class LevelLoader
{
    private static string PATH_TO_LEVELS_JSON = $"{Application.streamingAssetsPath}/levels.json";

    public LevelParams[] LevelsData { get; private set; }

    public async void LoadAllLevelsData(Action callback)
    {
        await LoadLevelsJson();
        callback?.Invoke();
    }

    public async Task LoadLevelsJson()
    {
        string json = await File.ReadAllTextAsync(PATH_TO_LEVELS_JSON);

        using (JsonTextReader jsonReader = new JsonTextReader(new StringReader(json)))
        {
            LevelsData = new JsonSerializer().Deserialize<LevelParams[]>(jsonReader);
        }
    }
}