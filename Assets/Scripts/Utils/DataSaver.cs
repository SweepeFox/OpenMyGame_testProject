using Newtonsoft.Json;
using UnityEngine;

public class DataSaver
{
    private const string SAVE_KEY = "save";

    public SaveData Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
        {
            return new SaveData()
            {
                level = 0,
                field = null
            };
        }

        return JsonConvert.DeserializeObject<SaveData>(PlayerPrefs.GetString(SAVE_KEY));
    }

    public void Save(SaveData saveData)
    {
        PlayerPrefs.SetString(SAVE_KEY, JsonConvert.SerializeObject(saveData));
    }
}