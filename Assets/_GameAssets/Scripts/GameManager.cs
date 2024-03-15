using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int CurrentLevel;
    public int RoundTime = 0;
    public bool IsFinishGame;
    public int RemainingCakes;
    public bool FinishStep;
    private float m_roundTimeDelta;
    
    private void OnEnable()
    {
        IsFinishGame = true;
        this.RegisterListener(EventID.OnPlayLevel,OnPlayLevel);
        this.RegisterListener(EventID.OnGoHomeScene,OnGoHomeScene);
        this.RegisterListener(EventID.OnFinishLoadScene,OnFinishLoadScene);
        this.RegisterListener(EventID.OnFinishGame,OnFinishGame);
        this.RegisterListener(EventID.OnReducedCake,OnReducedCake);
        this.RegisterListener(EventID.OnFinishStep,OnFinishStep);
        this.RegisterListener(EventID.OnMove,OnMove);
        this.RegisterListener(EventID.OnResetStep,OnResetStep);
        this.RegisterListener(EventID.OnUpdateCakeAlive,OnUpdateCakeAlive);
        this.RegisterListener(EventID.OnPlayNextLevel,OnPlayNextLevel);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnPlayLevel,OnPlayLevel);
        EventDispatcher.Instance.RemoveListener(EventID.OnGoHomeScene,OnGoHomeScene);
        EventDispatcher.Instance.RemoveListener(EventID.OnFinishLoadScene,OnFinishLoadScene);
        EventDispatcher.Instance.RemoveListener(EventID.OnFinishGame,OnFinishGame);
        EventDispatcher.Instance.RemoveListener(EventID.OnReducedCake,OnReducedCake);
        EventDispatcher.Instance.RemoveListener(EventID.OnPlayNextLevel,OnPlayNextLevel);
        EventDispatcher.Instance.RemoveListener(EventID.OnFinishStep,OnFinishStep);
        EventDispatcher.Instance.RemoveListener(EventID.OnMove,OnMove);
        EventDispatcher.Instance.RemoveListener(EventID.OnUpdateCakeAlive,OnUpdateCakeAlive);
        EventDispatcher.Instance.RemoveListener(EventID.OnResetStep,OnResetStep);
    }

    private void OnResetStep(object obj)
    {
        FinishStep = true;
        IsFinishGame = false;
        RoundTime = GameConfig.Instance.RoundTime.RoundTime;
        m_roundTimeDelta = RoundTime;
        this.PostEvent(EventID.OnChangeTime,RoundTime);
    }

    private void OnUpdateCakeAlive(object obj)
    {
        if(obj == null) return;
        RemainingCakes = (int)obj;
    }

    private void OnMove(object obj)
    {
        if(obj == null) return;
        FinishStep = false;
    }

    private void OnFinishStep(object obj)
    {
        FinishStep = true;
    }

    private void OnPlayNextLevel(object obj)
    {
        int level = CurrentLevel;
        if (!GameConfig.Instance || (GameConfig.Instance.CheckLatestLevelInData(level) && !GameConfig.Instance.CheckLatestLevel(level))) return;
        bool check = !GameConfig.Instance.CheckLatestLevel(level);
        if (!check)
        {
            level = 1;
            check =  !GameConfig.Instance.CheckLatestLevel(level);
        }
        else
        {
            level++;
        }
        
        if (check)
        {
            OnPlayLevel(level);
        } 
    }

    private void OnReducedCake(object obj)
    {
        RemainingCakes--;
        if (RemainingCakes <= 0)
        {
            int starCount = 3;

            if (GameConfig.Instance)
            {
                if (GameConfig.Instance.RoundTime.OneStarTimeThreshold >= RoundTime)
                {
                    starCount = 1;
                }else if (GameConfig.Instance.RoundTime.TwoStarTimeThreshold >= RoundTime)
                {
                    starCount = 2;
                }
            }
            
            this.PostEvent(EventID.OnFinishGame, starCount);
        }
    }

    private void OnFinishGame(object obj)
    {
        if(obj == null) return;
        IsFinishGame = true;
        if(GameConfig.Instance == null) return;
        LevelInfo levelInfo = new();
        levelInfo.Level = CurrentLevel;
        levelInfo.StarUnlock = (int)obj;
        levelInfo.IsUnlock = true;
        GameConfig.Instance.UpdateLevelData(levelInfo);
        FinishStep = false;
        StartCoroutine(DelayShowResult(levelInfo.StarUnlock,levelInfo.StarUnlock <= 0 ? 0 : .5f));
    }

    private IEnumerator DelayShowResult(int starCount,float TimeDelay)
    {
        yield return new WaitForSeconds(TimeDelay);
        this.PostEvent(EventID.OnShowResult,starCount);
    }

    private void OnFinishLoadScene(object obj)
    {
        if(obj == null) return;
        string nameScene = (string)obj;
        if(!GameConfig.Instance) return;
        if (GameConfig.Instance.NameHomeScene == nameScene)return;
        RemainingCakes = GameObject.FindGameObjectsWithTag("Cake").Length;
        RoundTime = GameConfig.Instance.RoundTime.RoundTime;
        m_roundTimeDelta = RoundTime;
        this.PostEvent(EventID.OnChangeTime,RoundTime);
        IsFinishGame = false;
        FinishStep = true;
    }

    private void Update()
    {
        if(IsFinishGame) return;
        if (RoundTime > 0)
        {
            m_roundTimeDelta -= Time.deltaTime;
            if (Mathf.CeilToInt(m_roundTimeDelta) < RoundTime)
            {
                RoundTime = Mathf.CeilToInt(m_roundTimeDelta);
                this.PostEvent(EventID.OnChangeTime,RoundTime);
                if (RoundTime == 0)
                {
                    this.PostEvent(EventID.OnFinishGame,0);
                }
            }
        }
    }
    private void OnGoHomeScene(object obj)
    {
        if (LoadSceneManager.Instance)
        {
            CurrentLevel = -1;
            LoadSceneManager.Instance.LoadScene(GameConfig.Instance.NameHomeScene);
        }
    }

    private void OnPlayLevel(object obj)
    {
        if(obj == null && GameConfig.Instance) return;
        if (LoadSceneManager.Instance)
        {
            FinishStep = false;
            CurrentLevel = (int)obj;
            LoadSceneManager.Instance.LoadScene(GameConfig.Instance.NameLevelScene + CurrentLevel);
        }
    }
}
