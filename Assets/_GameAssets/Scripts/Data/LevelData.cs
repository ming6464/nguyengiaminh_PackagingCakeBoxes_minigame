using System;
using System.Collections.Generic;

public class LevelData
{
    public List<LevelInfo>  m_levelInfos;

    public LevelInfo GetLevelInfo(int level)
    {
        foreach (LevelInfo levelInfo in m_levelInfos)
        {
            if (levelInfo.Level == level) return levelInfo;
        }
        return new LevelInfo() ;
    }

    public void UpdateLevel(LevelInfo levelInfo)
    {
        if (m_levelInfos == null) m_levelInfos = new();
        int index = m_levelInfos.FindIndex(x => x.Level == levelInfo.Level);
        if (index < 0)
        {
            m_levelInfos.Add(levelInfo);
            return;
        }

        m_levelInfos[index] = levelInfo;
    }
    
}

[Serializable]
public class LevelInfo
{
    public int Level;
    public bool IsUnlock;
    public int StarUnlock;
}