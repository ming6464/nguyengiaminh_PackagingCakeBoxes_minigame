using System.Collections.Generic;
using UnityEngine;

public static class DataSave
{
    public static bool FirstOpen
    {
        get => PlayerPrefs.GetInt("FirstOpen", 1) == 1;
        set => PlayerPrefs.SetInt("FirstOpen", value ? 1 : 0);
    }

    public static List<LevelInfo> LevelData
    {
        get
        {
            string text = PlayerPrefs.GetString("LevelData", null);
            if (string.IsNullOrEmpty(text)) return null;
            return JsonHelper.FromJsonList<LevelInfo>(text);
        }
        set
        {
            string text = JsonHelper.ToJson(value);
            PlayerPrefs.SetString("LevelData", text);
        }
    }
    
}