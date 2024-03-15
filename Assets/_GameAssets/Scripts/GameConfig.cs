using System;

public class GameConfig : Singleton<GameConfig>
{
    [Serializable]
    public struct RoundTimeInfo
    {
        public int RoundTime;
        public int OneStarTimeThreshold;
        public int TwoStarTimeThreshold;
    }
    
    public string NameLevelScene;
    public string NameHomeScene;
    public RoundTimeInfo RoundTime;
    
    private LevelData m_levelData;

    public override void Awake()
    {
        base.Awake();
        m_levelData = DataSave.LevelData;
        if (DataSave.FirstOpen)
        {
            DataSave.FirstOpen = false;
            LevelInfo levelInfo = new ();
            levelInfo.Level = 1;
            levelInfo.IsUnlock = true;
            levelInfo.StarUnlock = 0;
            m_levelData.UpdateLevel(levelInfo);
            DataSave.LevelData = m_levelData;
        }
    }

    public LevelInfo GetLevelInfoData(int level)
    {
        return m_levelData.GetLevelInfo(level);
    }
    
}