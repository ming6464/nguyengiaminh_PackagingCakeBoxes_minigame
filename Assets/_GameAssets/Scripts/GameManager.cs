using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float RoundTime = 0f;
    public bool IsFinishGame;
    
    private void OnEnable()
    {
        this.RegisterListener(EventID.OnPlayLevel,OnPlayLevel);
        this.RegisterListener(EventID.OnGoHomeScene,OnGoHomeScene);
        this.RegisterListener(EventID.OnResetLevel,OnResetLevel);
        this.RegisterListener(EventID.OnFinishLoadScene,OnFinishLoadScene);
    }

    private void OnFinishLoadScene(object obj)
    {
        if(obj == null) return;
        string nameScene = (string)obj;
        if(!GameConfig.Instance) return;
        if(GameConfig.Instance.NameHomeScene == nameScene) return;
        RoundTime = GameConfig.Instance.RoundTime.RoundTime;
    }

    private void Update()
    {
        if(IsFinishGame) return;
        if (RoundTime > 0)
        {
            RoundTime -= Time.deltaTime;
            
        }
    }

    private void OnResetLevel(object obj)
    {
        Debug.Log("Reset Level");
    }

    private void OnGoHomeScene(object obj)
    {
        if (LoadSceneManager.Instance)
        {
            LoadSceneManager.Instance.LoadScene(GameConfig.Instance.NameHomeScene);
        }
    }

    private void OnPlayLevel(object obj)
    {
        if(obj == null && GameConfig.Instance) return;
        if (LoadSceneManager.Instance)
        {
            LoadSceneManager.Instance.LoadScene(GameConfig.Instance.NameLevelScene + (int)obj);
        }
    }
}
