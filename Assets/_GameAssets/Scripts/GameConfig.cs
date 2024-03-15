using System;
using System.Collections.Generic;

public class GameConfig : Singleton<GameConfig>
{
    [Serializable]
    public struct RoundTimeInfo
    {
        public int RoundTime;
        public int TwoStarTimeThreshold;
        public int OneStarTimeThreshold;
    }
    
    public string NameLevelScene;
    public string NameHomeScene;
    public RoundTimeInfo RoundTime;
    public int LevelCount;
    public float MoveSpeedObjectGrid;
    
    private List<LevelInfo> m_levelInfos;

    public override void Awake()
    {
        base.Awake();
        m_levelInfos = DataSave.LevelData;
        if (DataSave.FirstOpen)
        {
            m_levelInfos = new List<LevelInfo>();
            DataSave.FirstOpen = false;
            LevelInfo levelInfo = new ();
            levelInfo.Level = 1;
            levelInfo.IsUnlock = true;
            levelInfo.StarUnlock = 0;
            m_levelInfos.Add(levelInfo);
            DataSave.LevelData = m_levelInfos;
        }
    }

    public LevelInfo GetLevelInfoData(int level)
    {
        foreach (LevelInfo levelInfo in m_levelInfos)
        {
            if (levelInfo.Level == level) return levelInfo;
        }

        return null;
    }

    public void UpdateLevelData(LevelInfo levelInfo)
    {
        if (m_levelInfos== null) return;
        int index = m_levelInfos.FindIndex(x => x.Level == levelInfo.Level);
        if (index < 0) return;
        m_levelInfos[index] = levelInfo;
         if (CheckLatestLevelInData(levelInfo.Level) && !CheckLatestLevel(levelInfo.Level))
         {
             LevelInfo levelInfoNew = new ();
             levelInfoNew.Level = levelInfo.Level + 1;
             levelInfoNew.IsUnlock = true;
             levelInfoNew.StarUnlock = 0;
             m_levelInfos.Add(levelInfoNew);
         }
         DataSave.LevelData = m_levelInfos;
    }

    public bool CheckLatestLevelInData(int level)
    {
        return level == m_levelInfos.Count;
    }

    public bool CheckLatestLevel(int level)
    {
        return level == LevelCount;
    }
    
}

[Serializable]
public class LevelInfo
{
    public int Level;
    public bool IsUnlock;
    public int StarUnlock;
}