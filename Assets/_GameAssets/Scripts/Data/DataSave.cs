using UnityEngine;

public static class DataSave
{
    public static bool FirstOpen
    {
        get => PlayerPrefs.GetInt("FirstOpen", 1) == 1;
        set => PlayerPrefs.SetInt("FirstOpen", value ? 1 : 0);
    }

    public static LevelData LevelData
    {
        get
        {
            string text = PlayerPrefs.GetString("LevelData", null);
            if (string.IsNullOrEmpty(text)) return new LevelData();
            return JsonHelper.FromJson<LevelData>(text);
        }
        set
        {
            string text = JsonHelper.ToJson(value);
            PlayerPrefs.SetString("LevelData", text);
        }
    }
    
}