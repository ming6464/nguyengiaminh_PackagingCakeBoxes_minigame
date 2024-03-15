using UnityEngine;

public class UIResultPanelScript : PopupAnimScript
{
    [SerializeField]
    private GameObject[] _startsOn;

    [SerializeField]
    private GameObject _levelFailedObj;
    
    [SerializeField]
    private GameObject _levelCompletedObj;

    [SerializeField]
    private GameObject _nextLevelButton;
    
    public void Home_button_on_click()
    {
        this.PostEvent(EventID.OnGoHomeScene);
    }

    public void Reset_button_on_click()
    {
        this.PostEvent(EventID.OnResetStep);
    }
    
    public void Next_button_on_click()
    {
        this.PostEvent(EventID.OnPlayNextLevel);
    }

    public void OnShowPanel(int startOnCount)
    {
        LoadStar(startOnCount);

        if (_levelFailedObj)
        {
            _levelFailedObj.SetActive(startOnCount < 1);
        }
        
        if (_levelCompletedObj)
        {
            _levelCompletedObj.SetActive(startOnCount > 0);
        }

        if (_nextLevelButton)
        {
            _nextLevelButton.SetActive(startOnCount > 0);
        }
        
    }

    private void LoadStar(int startOnCount)
    {
        for (int i = 0; i < _startsOn.Length; i++)
        {
            _startsOn[i]?.SetActive(startOnCount > i);
        }
    }
}
